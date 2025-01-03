from random import randint
import socket

def dhcp_request(socket, client_address, server_address, requested_ip_address):
    messageBytes = [
        0x01, 0x01, 0x06, 0x00,
        # random stream id
        randint(0, 255), randint(0, 255), randint(0, 255), randint(0, 255),
        0x00, 0x00, 0x00, 0x00, 0xc0, 0xa8, 0x03, 0x0f,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x10, 0xff, 0xe0, 0x45,
        0x3c, 0xcb, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x63, 0x82, 0x53, 0x63,
        0x35, 0x01, 0x03, 0x3d, 
        0x07, 0x01, 0x10, 0xff, 0xe0, 0x45, 0x3c, 0xcb,
        0x0c, 0x0f, 0x44, 0x45, 0x53, 0x4b, 0x54, 0x4f,
        0x50, 0x2d, 0x54, 0x4a, 0x47, 0x55, 0x43, 0x48,
        0x34, 0x51, 0x12, 0x00, 0x00, 0x00, 0x44, 0x45,
        0x53, 0x4b, 0x54, 0x4f, 0x50, 0x2d, 0x54, 0x4a,
        0x47, 0x55, 0x43, 0x48, 0x34,
        # requested ip
        50, 4, 192, 168, 3, 1,
        0xff
    ]

    client.bind(client_address)
    socket.sendto(bytes(messageBytes), server_address)

def dhcp_inform(socket, client_address, server_address):
    messageBytes = [
        0x01, 0x01, 0x06, 0x00,
        # random stream id
        randint(0, 255), randint(0, 255), randint(0, 255), randint(0, 255),
        0x00, 0x00, 0x00, 0x00, 0xc0, 0xa8, 0x03, 0x0f,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x10, 0xff, 0xe0, 0x45,
        0x3c, 0xcb, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x63, 0x82, 0x53, 0x63,
        0x35, 0x01, 0x08, 0x3d, 
        0x07, 0x01, 0x10, 0xff, 0xe0, 0x45, 0x3c, 0xcb,
        0x0c, 0x0f, 0x44, 0x45, 0x53, 0x4b, 0x54, 0x4f,
        0x50, 0x2d, 0x54, 0x4a, 0x47, 0x55, 0x43, 0x48,
        0x34, 0x51, 0x12, 0x00, 0x00, 0x00, 0x44, 0x45,
        0x53, 0x4b, 0x54, 0x4f, 0x50, 0x2d, 0x54, 0x4a,
        0x47, 0x55, 0x43, 0x48, 0x34, 0x3c, 0x08, 0x4d,
        0x53, 0x46, 0x54, 0x20, 0x35, 0x2e, 0x30, 0x37,
        0x0e, 0x01, 0x03, 0x06, 0x0f, 0x1f, 0x21, 0x2b,
        0x2c, 0x2e, 0x2f, 0x77, 0x79, 0xf9, 0xfc, 0xff
    ]

    client.bind(client_address)
    client.sendto(bytes(messageBytes), server_address)

client = socket.socket(socket.AddressFamily.AF_INET, socket.SOCK_DGRAM)
# dhcp_inform(client, ("192.168.3.15", 68), ("192.168.3.1", 67))

requested_ip = 3232235873
dhcp_request(client, ("192.168.3.15", 68), ("192.168.3.1", 67), requested_ip)
