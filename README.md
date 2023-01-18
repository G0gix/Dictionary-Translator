# Dictionary-Translator

## Цель проекта
Целью проекта является облегчить изучение английского языка, в ходе прочтения книг или документаций. <br/>
Решением является автоматизация перевода слов, и добавления их в личный словарь. <br/>

Доступный функционал:
* Перевод указанного слова
* Указание транскрипции
* Ссылка на произношение слова
* Ссылка на примеры предложений с данным словом


## Видео демонстрация
https://user-images.githubusercontent.com/73593386/213146718-449d68a8-6886-40b4-8deb-7e4fcf3e20e4.mp4

## Используемые технологии
 * Google Sheets - Google Таблица является основным интерфейсом взаимодействия пользователя.
 * [Google Apps Script](https://www.google.com/script/start/) - Целью Google Script является обработка добавления новых элементов в Google таблицу, 
 и отправка данных на API.
 * ASP Web API - Используется для получения запросов на перевод, и вставку данных в Google таблицу. *Является основным компонентом обработки данных приложения*.
 * [Yandex Dictionary API](https://yandex.com/dev/dictionary/) - Данный API используется для перевода и получения транскрипции.
 * [Google Sheets API](https://developers.google.com/sheets/api/guides/concepts) - С помощью данного API происходит вставка данных в Google Таблицу.
 
 ## Как использовать?
 Если вы хотите использовать данное приложение, вам требуется пройти несколько пунктов.
 
### 1. Добавление Google Script к вашей Google таблице
 Для того, чтобы получить данные, введенные пользователем в таблицу Google, нам нужно добавить скрипт обработки изменения ячеек.
 В корне проекта располагается файл GoogleSheetScript.js, который вам нужно разместить в вашу Google таблицу.
 
 [Как добавить скрипт к Google Таблице](https://developers.google.com/apps-script/guides/sheets/functions)
 
 ```javascript
  let url = "https://url/TranslateConstroller/translate"
 ```
 В данной строке кода вместо url должен быть указан ваш адрес, по которому будет размещаться ASP Web API. <br/>
 [Или вы можете использовать ngrok](https://ngrok.com/) 
 
### 2. Получение Google Credentials для доступа к Google Sheets
[Как получить service-account credentials](https://developers.google.com/workspace/guides/create-credentials#service-account)

### 3. [Получить Dictionary API Key](https://yandex.com/dev/dictionary/)

### 4. Указать полученные данные в Dictionary-Translator/appsettings.json
Введите полученные вами данные в json.file

``` json
"YandexTranslateApiKey": "",

"GoogleSheetsOptions": {
    "ApplicationName": "",
    "SheetID": "",
    "SheetName": "",
    "SecretsPath": "",
    "SheetRange": {
       "ColumnStart": "",
       "ColumnEnd": ""
    }
  },
```
* YandexTranslateApiKey - API ключ из пункта 3.

* ApplicationName - [Ваше названия проекта в cloud.google](https://console.cloud.google.com/) 
* SheetID - Можно получить из адресной строки вашей Google таблицы
* SheetName - Наименование листа вашей Google таблицы
* SecretsPath - Путь к json файлу полученному из пункта 2.
* ColumnStart - Наименование столбца начала вашей гугл таблицы в виде буквы. Пример буква (А).
* ColumnEnd - Данный столбец должен быть на 5 больше от ColumnStart. Например, если вы выбрали столбец А, ColumnEnd должен быть F.

