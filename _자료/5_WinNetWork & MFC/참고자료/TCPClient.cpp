
// TCPClient.cpp : 이 파일에는 'main' 함수가 포함됩니다. 거기서 프로그램 실행이 시작되고 종료됩니다.
//
#define _WINSOCK_DEPRECATED_NO_WARNINGS
#pragma comment(lib, "ws2_32.lib")
#include <iostream> 
#include <WinSock2.h>

#define MAX_BUFFER_SIZE 2048

int main()
{
    // 윈속 초기화.
    WSADATA wsa;
    if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0) return -1;

    // socket() 소켓 생성.
    SOCKET sock = socket(AF_INET, SOCK_STREAM, 0);
    if (INVALID_SOCKET == sock) exit(-1);

    // 서버 정보 객체 설정.
    SOCKADDR_IN serveraddr;
    ZeroMemory(&serveraddr, sizeof(serveraddr));
    serveraddr.sin_family = AF_INET;
    serveraddr.sin_port = htons(9000);
    serveraddr.sin_addr.S_un.S_addr = inet_addr("127.0.0.1");

    // connect()
    if (SOCKET_ERROR == connect(sock, (SOCKADDR*)&serveraddr, sizeof(serveraddr))) exit(-1);

    int len, retval;
    char buf[MAX_BUFFER_SIZE + 1];
    // 서버와 데이터 통신.
    while (1)
    {
        // 데이터 입력.
        ZeroMemory(buf, sizeof(buf));
        printf("\n[보낼 데이터]");
        if (fgets(buf, MAX_BUFFER_SIZE + 1, stdin) == NULL) break;

        // '\n' 문자 제거.
        len = strlen(buf);
        if (buf[len - 1] == '\n') buf[len - 1] = '\0';
        if (strlen(buf) == 0) break;

        // 데이터 보내기.
        retval = send(sock, buf, strlen(buf), 0);
        if (SOCKET_ERROR == retval)
        {
			printf("send()");
            break;
        }

        printf("[TCP 클라이언트] %d바이트를 보냈습니다.\n", retval);

        // 데이터 받기.
		ZeroMemory(buf, sizeof(buf));
        retval = recv(sock, buf, retval, 0);
        if (SOCKET_ERROR == retval)
        {
			printf("recv()");
            break;
        }
        else if (0 == retval) break;

        // 받은 데이터 출력.
        buf[retval] = '\0';
        printf("[TCP 클라이언트] %d바이트를 받았습니다.\n", retval);
        printf("[받은 데이터]%s\n", buf);
    }

    closesocket(sock);

    WSACleanup();
}