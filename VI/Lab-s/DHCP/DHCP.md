# DHCP

## 0. Источники

RFCs: 1541, 2131, 2132.

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

## 3. Проследить цепочки DHCP после отправки сообщения DHCP release. Отдельно отследить DHCP сообщения после выполнения команды `Renew`

### DHCP release

![DHCP release](image-5.png)

### Renew

В отличии от обычного DHCP discover, в данном случае присутствует опция № 50, запрошенного IP-адреса, который раннее был выдан DHCP-клиенту.

![DHCP. Renew. Discover](image-6.png)

В остальном (offer, request, acknowledgement) сообщения не отличаются от первичной выдачи адреса.

## 4. Проанализировать сообщения DHCPINFORM при разных конфигурациях DHCP сервера

TODO: попроси помощи у коллег;

## 5. Смоделировать ситуации для генерации сообщения DHCPNACK

### a. Занятый адрес

TODO: пофикси ARP проверку сервера перед выдачей адреса

### b. Отсутствие свободных адресов

## 6. Смоделировать ситуации для генерации сообщения DHCPDECLINE

### a. Отдельно отследить работу ARP в ситуации с сообщением DHCPDECLINE

## 7. Анализ продления аренды

### a. Перезагрузка клиента

### b. Когда время аренды составляет $1/2$, $7/8$. При условии, что сервер не откликается

## 8. Анализ цепочки связанной с получением адреса при условии, что DHCP сервер не откликается

## 9. Анализ DHCP сообщений при наличии в одном широковещательном домене нескольких DHCP серверов

## 10. Провести анализ заголовка BOOTP всех сообщений при всех ситуациях DHCP сообщений

## 11. Моделирование DHCP опций

| Option | Name  |
| :----: | :---: |

# Используемые материалы

- [RFC 2131](https://www.ietf.org/rfc/rfc2131.txt);
- [RFC 2132](https://www.ietf.org/rfc/rfc2132.txt);
- [protokols.ru -RFC 2131](https://www.protokols.ru/WP/wp-content/uploads/1997/03/rfc2131.pdf);
- [protokols.ru -RFC 2132](https://www.protokols.ru/WP/wp-content/uploads/1997/03/rfc2132.pdf);
- [O'Reilly. DHCP for Windows 2000](https://docs.yandex.ru/docs/view?tm=1730686489&tld=ru&lang=en&name=OReilly.Dhcp.For%20Windows%202000.pdf&text=dhcp%20full%20book%20.pdf&url=https%3A%2F%2Ftheswissbay.ch%2Fpdf%2FGentoomen%2520Library%2FProgramming%2FMisc%2FOReilly.Dhcp.For%2520Windows%25202000.pdf&lr=68&mime=pdf&l10n=ru&sign=83c5fca5eb5067ee17b6884035e46196&keyno=0&nosw=1&serpParams=tm%3D1730686489%26tld%3Dru%26lang%3Den%26name%3DOReilly.Dhcp.For%2520Windows%25202000.pdf%26text%3Ddhcp%2Bfull%2Bbook%2B.pdf%26url%3Dhttps%253A%2F%2Ftheswissbay.ch%2Fpdf%2FGentoomen%252520Library%2FProgramming%2FMisc%2FOReilly.Dhcp.For%252520Windows%2525202000.pdf%26lr%3D68%26mime%3Dpdf%26l10n%3Dru%26sign%3D83c5fca5eb5067ee17b6884035e46196%26keyno%3D0%26nosw%3D1);