# DHCP

## 1. Настройка DHCP-сервера

Ниже будет указана базовая настройка DHCP-сервера в консоли роутера Cisco 3745 в GNS3.

```console
## (a) Определение пула адресов
## Переходим в режим настройки dhcp пула с помощью команды `ip dhcp pool [pool-name]`
R1(config)#ip dhcp pool lan0
## С помощью команды `network [ip-address] [subnet-mask]` настраиваем пул выдаваемых адресов
## (Не забудьте настроить статический IP-адрес на порту роутера)
R1(dhcp-config)#network 192.168.1.0 255.255.255.0
## (b) Определение аренды
## Для этого воспользуемся командой `lease [days: 0-365] [hours: 0-23] [minutes: 0-59]`
R1(dhcp-config)#lease 0 10 0
## (c) Задать шлюз
## Для этого воспользуемся командой `default-router [gateway-ip-address]`
R1(dhcp-config)#default-router 192.168.1.1
## (d) Задать DNS адреса
## Для этого воспользуемся командой `dns-server [dns-ip-1] [dns-ip-2] ... [dns-ip-n]`
R1(dhcp-config)#dns-server 1.1.1.1 77.88.8.8 8.8.8.8
```

## 2. При помощи анализатора трафика проследить цепочки DHCP сообщений

### DHCP discover -> DHCP offer

![dhcp discover](image-1.png)
![dhcp offer](image-2.png)

### DHCP request -> DHCP acknowledgement

![dhcp request](image-3.png)
![dhcp acknowledgement](image-4.png)