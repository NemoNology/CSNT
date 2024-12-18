# DNS клиент и сервер

Курсовая работа на тему "DNS клиент и сервер" реализованная на Rust.

## Принцип работы

### Обмен сообщениями

```mermaid
sequenceDiagram
    Client->>Server: Запрос('oizi.dms.zab')
    alt Запись найдена
        Server->>Client: Ответ('10.11.3.139')
    else Запись не найдена
        Server->>Client: Ответ('Ничего не найдено')
    end
```

## Структуры

### DNS запрос

- идентификатор запроса: 16 бит;
- разрешаемое доменное имя: строка;

### DNS ответ

- идентификатор запроса: 16 бит;
- разрешённый адрес: IPv4 / none;

### DNS запись

- доменное имя: строка;
- адрес: IPv4;

### Данные пакета

- идентификатор запроса: 16 бит;
- DNS запрос: DNS запрос;
- DNS ответ: DNS ответ / none;

### DNS сервер

- таблица DNS записей: коллекция DNS записей;
- UDP сокет: UDP сокет;

### DNS клиент

- UDP сокет: UDP сокет;
- коллекция кешированных DNS записей: коллекция DNS записей;

### Контроллер таблицы DNS записей

- путь к файлу таблицы записей: String;
