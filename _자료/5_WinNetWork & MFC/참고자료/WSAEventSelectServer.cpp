#define _WINSOCK_DEPRECATED_NO_WARNINGS
#pragma comment(lib, "ws2_32.lib")
#include <WinSock2.h>
#include <stdio.h>

#define BUFSIZE 512

struct SOCKETINFO
{
    SOCKET sock;
    char buf[BUFSIZE + 1];
    int recvbytes;
    int sendbytes;
    BOOL recvdelayed;
    SOCKETINFO* next;
};

int g_nTotalSockets = 0;
SOCKETINFO* arrSocketInfo[WSA_MAXIMUM_WAIT_EVENTS];
WSAEVENT arrEvent[WSA_MAXIMUM_WAIT_EVENTS];

BOOL AddSocketInfo(SOCKET sock);
VOID RemoveSocketInfo(int index);

void err_quit(const char* msg);
void err_display(const char* msg);
void err_display(const int& errcode);

int main(int argc, char* argv[])
{
    // 윈속 초기화.
    WSADATA wsa;
    if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0) return -1;

    // socket()
    SOCKET listen_sock = socket(AF_INET, SOCK_STREAM, 0);
    if (INVALID_SOCKET == listen_sock)
    {
        closesocket(listen_sock);
        err_quit("socket()");
    }

    // 소켓 정보 추가.
    if (FALSE == AddSocketInfo(listen_sock)) return -1;

    // WSAEventSelect()
    int retval = WSAEventSelect(listen_sock, arrEvent[g_nTotalSockets - 1], FD_ACCEPT | FD_CLOSE);
    if (SOCKET_ERROR == retval)
    {
        closesocket(listen_sock);
        err_quit("WSAEventSelect()");
    }

    // bind()
    SOCKADDR_IN serveraddr;
    ZeroMemory(&serveraddr, sizeof(serveraddr));
    serveraddr.sin_family = AF_INET;
    serveraddr.sin_port = htons(9000);
    serveraddr.sin_addr.S_un.S_addr = htonl(INADDR_ANY);
    retval = bind(listen_sock, (SOCKADDR*)&serveraddr, sizeof(serveraddr));
    if (SOCKET_ERROR == retval)
    {
        closesocket(listen_sock);
        err_quit("bind()");
    }

    // listen()
    retval = listen(listen_sock, SOMAXCONN);
    if (SOCKET_ERROR == retval)
    {
        closesocket(listen_sock);
        err_quit("listen()");
    }

    int index;
    WSANETWORKEVENTS NetworkEvents;
    SOCKET client_sock;
    SOCKADDR_IN clientaddr;
    int addlen;
    while (true)
    {
        // 이벤트 객체 관찰.
        index = WSAWaitForMultipleEvents(g_nTotalSockets, arrEvent, FALSE, WSA_INFINITE, FALSE);
        if (WSA_WAIT_FAILED == index)
        {
            err_display("WSAWaitForMultipleEvents()");
            continue;
        }

        index -= WSA_WAIT_EVENT_0;

        // 구체적인 네트워크 이벤트 알아내기.
        retval = WSAEnumNetworkEvents(arrSocketInfo[index]->sock, arrEvent[index], &NetworkEvents);
        if (SOCKET_ERROR == retval)
        {
            err_display("WSAEnumNetworkEvents()");
            continue;
        }

        // FD_ACCEPT 이벤트 처리.
        if (NetworkEvents.lNetworkEvents & FD_ACCEPT)
        {
            if (0 != NetworkEvents.iErrorCode[FD_ACCEPT_BIT])
            {
                err_display(NetworkEvents.iErrorCode[FD_ACCEPT_BIT]);
                continue;
            }

            // accept()
            addlen = sizeof(clientaddr);
            client_sock = accept(arrSocketInfo[index]->sock, (SOCKADDR*)&clientaddr, &addlen);
            if (INVALID_SOCKET == client_sock)
            {
                err_display("accept()");
                continue;
            }
            printf("[TCP 서버] 클라이언트 접속 : IP 주소=%s, 포트 번호=%d\n",
                inet_ntoa(clientaddr.sin_addr),
                ntohs(clientaddr.sin_port));

            // 접속 수 제한.
            if (WSA_MAXIMUM_WAIT_EVENTS <= g_nTotalSockets)
            {
                printf("[오류] 더 이상 접속을 받아들일 수 없습니다!\n");
                closesocket(client_sock);
                continue;
            }

            // 클라이언트 소켓 정보 추가.
            if (FALSE == AddSocketInfo(client_sock)) continue;

            // WSAEventSelect()
            retval = WSAEventSelect(client_sock, arrEvent[g_nTotalSockets - 1], FD_READ | FD_WRITE | FD_CLOSE);
            if (SOCKET_ERROR == retval) err_quit("WSAEventSelect()");
        }

        // FD_READ, FD_WRITE 이벤트 처리.
        if (NetworkEvents.lNetworkEvents & FD_READ ||
            NetworkEvents.lNetworkEvents & FD_WRITE)
        {
            if (NetworkEvents.lNetworkEvents & FD_READ &&
                0 != NetworkEvents.iErrorCode[FD_READ_BIT])
            {
                err_display(NetworkEvents.iErrorCode[FD_READ_BIT]);
                continue;
            }
            if (NetworkEvents.lNetworkEvents & FD_WRITE &&
                0 != NetworkEvents.iErrorCode[FD_WRITE_BIT])
            {
                err_display(NetworkEvents.iErrorCode[FD_WRITE_BIT]);
                continue;
            }

            SOCKETINFO* ptr = arrSocketInfo[index];
            if (0 == ptr->recvbytes)
            {
                // 데이터 받기.
                retval = recv(ptr->sock, ptr->buf, BUFSIZE, 0);
                if (SOCKET_ERROR == retval)
                {
                    if (WSAEWOULDBLOCK != WSAGetLastError())
                    {
                        err_display("recv()");
                        RemoveSocketInfo(index);
                    }
                    continue;
                }

                ptr->recvbytes = retval;
                
                // 받은 데이터 출력.
                ptr->buf[retval] = '\0';
                addlen = sizeof(clientaddr);
                getpeername(ptr->sock, (SOCKADDR*)&clientaddr, &addlen);
                printf("[TCP/%s:%d]%s\n",
                    inet_ntoa(clientaddr.sin_addr),
                    ntohs(clientaddr.sin_port),
                    ptr->buf);
            }

            if (ptr->recvbytes > ptr->sendbytes)
            {
                retval = send(ptr->sock, ptr->buf + ptr->sendbytes, ptr->recvbytes - ptr->sendbytes, 0);
                if (SOCKET_ERROR == retval)
                {
                    if (WSAEWOULDBLOCK != WSAGetLastError())
                    {
                        err_display("send()");
                        RemoveSocketInfo(index);
                    }
                    continue;
                }

                ptr->sendbytes += retval;
                // 받은 데이터를 모두 보냈는지 확인.
                if (ptr->recvbytes == ptr->sendbytes)
                {
                    ptr->recvbytes = ptr->sendbytes = 0;
                }
            }
        }


        // FD_ACCEPT 이벤트 처리.
        if (NetworkEvents.lNetworkEvents & FD_CLOSE)
        {
            if (0 != NetworkEvents.iErrorCode[FD_CLOSE_BIT])
            {
                err_display(NetworkEvents.iErrorCode[FD_CLOSE_BIT]);
            }
            RemoveSocketInfo(index);
        }
    }

    // 윈속 종료.
    WSACleanup();

    return 0;
}

// 소켓 정보 추가.
BOOL AddSocketInfo(SOCKET sock)
{
    if (WSA_MAXIMUM_WAIT_EVENTS <= g_nTotalSockets)
    {
        printf("[오류] 소켓 정보를 추가할 수 없습니다!\n");
        return FALSE;
    }

    SOCKETINFO* ptr = new SOCKETINFO;
    if (NULL == ptr)
    {
        printf("[오류] 메모리가 부족합니다!\n");
        return FALSE;
    }

    WSAEVENT hEvent = WSACreateEvent();
    if (WSA_INVALID_EVENT == hEvent)
    {
        err_display("WSACreateEvent()");
        return FALSE;
    }

    ptr->sock = sock;
    ptr->recvbytes = 0;
    ptr->sendbytes = 0;
    arrSocketInfo[g_nTotalSockets] = ptr;
    arrEvent[g_nTotalSockets] = hEvent;
    g_nTotalSockets++;

    return TRUE;
}
// 소켓 정보 삭제.
VOID RemoveSocketInfo(int index)
{
    SOCKETINFO* ptr = arrSocketInfo[index];
    
    // 클라이언트 정보 얻기.
    SOCKADDR_IN addr;
    int addlen = sizeof(addr);
    getpeername(ptr->sock, (SOCKADDR*)&addr, &addlen);
    printf("[TCP 서버] 클라이언트 종료 : IP 주소=%s, 포트 번호=%d\n", inet_ntoa(addr.sin_addr), ntohs(addr.sin_port));

    closesocket(ptr->sock);
    delete ptr;
    WSACloseEvent(arrEvent[index]);

    for (int i = index; i < g_nTotalSockets; i++)
    {
        arrSocketInfo[i] = arrSocketInfo[i + 1];
        arrEvent[i] = arrEvent[i + 1];
    }

    g_nTotalSockets--;
}

// 메시지 박스, 에러 메시지 출력 후 종료.
void err_quit(const char* msg)
{
    LPVOID lpMsgBuf;
    FormatMessage(
        FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM,
        NULL, WSAGetLastError(),
        MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
        (LPTSTR)&lpMsgBuf, 0, NULL);

    // msg : 메시지 박스의 타이틀(Caption)
    MessageBox(NULL, (LPCTSTR)lpMsgBuf, msg, MB_ICONERROR);

    // 메모리 해제, 핸들 무효화.
    LocalFree(lpMsgBuf);

    exit(-1);
}
// 에러 메시지 출력.
void err_display(const char* msg)
{
    LPVOID lpMsgBuf;
    FormatMessage(
        FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM,
        NULL, WSAGetLastError(),
        MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
        (LPTSTR)&lpMsgBuf, 0, NULL);

    printf("[%s] %s", msg, (LPCTSTR)lpMsgBuf);

    LocalFree(lpMsgBuf);
}
// 에러 메시지 출력.
void err_display(const int& errcode)
{
    LPVOID lpMsgBuf;
    FormatMessage(
        FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM,
        NULL, errcode,
        MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
        (LPTSTR)&lpMsgBuf, 0, NULL);

    printf("[오류] %s", (LPCTSTR)lpMsgBuf);

    LocalFree(lpMsgBuf);
}