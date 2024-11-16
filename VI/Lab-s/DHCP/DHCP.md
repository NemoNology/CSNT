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

Для того, чтобы обнаружить dhcp-сервер в сети, dhcp-клиент отправляет сообщение обнаружения (`DISCOVER`) на *broadcast*  MAC и IP адреса по UDP порту 68 на порт 67. DHCP-сервер в ответ отправляет предложение адреса (`OFFER`) по *unicast* и UDP порту 67 на порт 68, с предлагаемым адресом клиенту по UDP.

![dhcp discover](image-1.png)
![dhcp offer](image-2.png)

### DHCP request -> DHCP acknowledgement

На предложение клиент отвечает запросом (`REQUEST`) или отказом от предложения (`DECLINE`). В случае `REQUEST`, сервер отвечает `ACKNOWLEDGEMENT` (`ACK`) или `NEGATIVE ACKNOWLEDGEMENT` (`NAK`), в случае отказа выдачи запрошенного клиентом адреса.

![dhcp request](image-3.png)
![dhcp acknowledgement](image-4.png)

Когда сервер посылает `ACKNOWLEDGEMENT`, завершается цикл `DORA`, названный в честь первых букв указанных выше *успешных* сообщений. А завершение этого цикла означает успешное получение адреса.

*Также есть сообщения из группы `RIND` - `RELEASE`, `INFORM`, `NEGATIVE ACKNOWLEDGEMENT` и `DECLINE`*.

## 3. Проследить цепочки DHCP после отправки сообщения DHCP release. Отдельно отследить DHCP сообщения после выполнения команды `Renew`

### DHCP release

> DHCPRELEASE - Клиент сообщает серверу об освобождении сетевого адреса и отказе от аренды. [3]

`PC1> dhcp -x`

![DHCP release](image-5.png)

### Renew

В отличии от обычного DHCP discover, в данном случае присутствует опция № 50, запрошенного IP-адреса, который раннее был выдан DHCP-клиенту.

`PC1> dhcp -r`

![DHCP. Renew. Discover](image-6.png)

В остальном (offer, request, acknowledgement) сообщения не отличаются от первичной выдачи адреса.

## 4. Проанализировать сообщения DHCPINFORM при разных конфигурациях DHCP сервера

> `DHCPINFORM`Клиент запрашивает у сервера только конфигурационные параметры, поскольку уже имеет сетевой адрес, заданный другим способом. [3]

### Попытка с использованием `udhcpc` на `Alpine Linux`

```
/ # udhcpc --help
Usage: udhcpc ...

        ...

        -o              Don't request any options (unless -O is given)
        -O OPT          Request option OPT from server (cumulative)
        -x OPT:VAL      Include option OPT in sent packets (cumulative)
                        Examples of string, numeric, and hex byte opts:
                        -x hostname:bbox - option 12
                        -x lease:3600 - option 51 (lease time)
                        -x 0x3d:0100BEEFC0FFEE - option 61 (client id)
                        -x 14:'"dumpfile"' - option 14 (shell-quoted)

        ...

/ # udhcpc -o -x 53:8
udhcpc: started, v1.36.1
udhcpc: broadcasting discover
udhcpc: broadcasting select for 192.168.1.9, server 192.168.1.1
udhcpc: lease of 192.168.1.9 obtained from 192.168.1.1, lease time 36000 
```

Итог

![DORA, вместо DHCPINFORM](image-7.png)

### Попытка с использованием `ipconfig` на `Windows 10`

```
PS C:\Users\NemoNology> ipconfig /setclassid “Ethernet” DHCPINFORM

Настройка протокола IP для Windows

Код класса DHCPv4 для адаптера Ethernet успешно установлен.
```

![ipconfig /setclassid “Ethernet” DHCPINFORM](image-8.png)

Наличие опции под кодом `77`, которого нет в RFC 2132, подтверждает, что `ipconfig` использует расширения DHCP.

#### Использование UDP сокета на Python3 с байтами, основанными на байтах показанного выше пакета

![DHCP INFORM by Python3 UDP socket](image-13.png)
![DHCP ACK for DHCP INFORM](image-14.png)

### Попытка с использованием `dhcping` на `Alpine Linux`

```
/ # dhcping -c 192.168.1.2 -s 192.168.1.1 -i
Got answer from: 192.168.1.1
```

![DHCP INFORM](image-11.png)
![DHCP ACK как ответ на DHCP INFORM](image-12.png)

## 5. Смоделировать ситуации для генерации сообщения DHCPNACK

> `DHCPNAK` - Сервер сообщает клиенту о непригодности указанного тем сетевого адреса (например, при переносе клиента в другую подсеть) или окончании аренды для этого клиента. [3]

### a. DHCP REQUEST c опцией `55` от клиента одной подсети в другую подсеть

Получаем IP-адрес от DHCP-сервера одной подсети, а затем переносим клиента в другую подсеть.

![DHCP REQUEST for subnet information](image-15.png)
![DHCP NAK for DHCP REQUEST from another subnet](image-16.png)

### b. DHCP REQUEST с опцией `50` с запросом занятого IP-адреса

Формируем DHCP REQUEST, с опцией запрошенного IP-адреса (опция `50`), и в виде запрошенного адреса указываем, например, адрес DHCP-сервера;

![DHCP REQUEST с запрошенным занятым адресом](image-17.png)
![DHCP NAK в ответ на запрошенный занятый адрес](image-18.png)

## 6. Смоделировать ситуации для генерации сообщения DHCPDECLINE

> `DHCPDECLINE` - Клиент сообщает серверу о том, что сетевой адрес уже занят. [3]



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