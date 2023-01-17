async function onChange() {
    let activeSheet = SpreadsheetApp.getActiveSpreadsheet().getActiveSheet();
    let cell = activeSheet.getActiveCell();
    let value = cell.getValue();

    let row = cell.getRow();
    let column = cell.getColumn();

    if (column !== 1) {
        return;
    }

    if (value === null || value === undefined || value === "" || value === " ") {
        return;
    }

    Logger.log(value)

    let url = "https://url/TranslateConstroller/translate"

    var formData = {
        'valueToTranslate': value,
        'rowIndex': row
    };

    var options = {
        'method': 'post',
        'payload': formData
    };

    UrlFetchApp.fetch(url, options);
}