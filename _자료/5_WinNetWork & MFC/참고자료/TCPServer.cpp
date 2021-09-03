// Network.cpp : 이 파일에는 'main' 함수가 포함됩니다. 거기서 프로그램 실행이 시작되고 종료됩니다.
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
    SOCKET listen_socket = socket(AF_INET, SOCK_STREAM, 0);
    if (INVALID_SOCKET == listen_socket) exit(-1);

    // 서버 정보 객체 설정.
    // INADDR_ANY : 서버의 IP 주소를 자동으로 찾아서 대입해주는 함수(복잡한 #define문의로 정의 됨)
    // 서버가 두 개 이상의 IP를 가진 경우(multihomed host)가 아니면 해당 값을 사용.
    SOCKADDR_IN serveraddr;
    // 해당 메모리에 있을 쓰래기 값을 0으로 지워준다.
    ZeroMemory(&serveraddr, sizeof(serveraddr));
    serveraddr.sin_family = AF_INET;
    serveraddr.sin_port = htons(9000);
    serveraddr.sin_addr.S_un.S_addr = htonl(INADDR_ANY);

    // bind() 소켓 설정.
    if (bind(listen_socket, (SOCKADDR*)&serveraddr, sizeof(serveraddr)) == SOCKET_ERROR)
    {
        closesocket(listen_socket);
        WSACleanup();
		printf("bind()");
		exit(-1);
    }

    // listen() 수신 대기열 생성.
    // SOMAXCONN : Socket Max Connection
    if (listen(listen_socket, SOMAXCONN) == SOCKET_ERROR)
    {
        closesocket(listen_socket);
        WSACleanup();
		printf("listen()");
		exit(-1);
    }

    // 데이터 통신에 사용할 변수.
    SOCKADDR_IN clientaddr;
    int addrlen = sizeof(SOCKADDR_IN);
    ZeroMemory(&clientaddr, addrlen);

    SOCKET client_socket;    
    int retval;
    char buf[MAX_BUFFER_SIZE + 1];
    while (1)
    {
        // accept() 연결 대기.
        client_socket = accept(listen_socket, (SOCKADDR*)&clientaddr, &addrlen);
        if (INVALID_SOCKET == client_socket)
        {
			printf("accept()");
            continue;
        }

        printf("\n[TCP 서버] 클라이언트 접속 : IP 주소=%s, 포트 번호=%d\n",
            inet_ntoa(clientaddr.sin_addr),
            ntohs(clientaddr.sin_port));

        // 데이터 통신.
        while (1)
        {
            ZeroMemory(buf, MAX_BUFFER_SIZE + 1);
            // 데이터 받기.
            retval = recv(client_socket, buf, MAX_BUFFER_SIZE, 0);
            if (SOCKET_ERROR == retval)
            {
				printf("recv()");
                break;
            }
            else if (0 == retval) break;

            // 받은 데이터 출력.
            buf[retval] = '\0';
            printf("\n[TCP/%s:%d]%s\n",
                inet_ntoa(clientaddr.sin_addr),
                ntohs(clientaddr.sin_port),
                buf);

            // 데이터 보내기.
            retval = send(client_socket, buf, retval, 0);
            if (SOCKET_ERROR == retval)
            {
				printf("send()");
                break;
            }
        }

        // closesocket().
        // 클라이언트 소켓 종료.
        closesocket(client_socket);
        printf("\n[TCP 서버] 클라이언트 종료 : IP 주소=%s, 포트 번호=%d\n",
            inet_ntoa(clientaddr.sin_addr),
            ntohs(clientaddr.sin_port));
    }

    // closesocket().
    // 리슨 소켓 종료.
    closesocket(listen_socket);

    // 윈속 종료.
    WSACleanup();
}