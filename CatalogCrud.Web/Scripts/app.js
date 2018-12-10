function searchFields() {
    var field = $('#search-input-value').val();
    var token = $('input[name="__RequestVerificationToken"]').val();
    var searchPrependText = $('#searching').text();

    $('#searching').text('Идет поиск...');

    $.ajax({
        url: '/Field/FindFields',
        type: 'Post',
        data: {
            __RequestVerificationToken: token,
            "value": field
        },
        success: function (html) {
            $('#searching').text(searchPrependText);
            $('#found-items').html(html);
        },
        error: function (XmlHttpRequest) {
            $('#searching').text = 'Ошибка';
            console.log(XmlHttpRequest);
        }
    });
    return false;
}

function attachField(fieldId) {
    if (document.body.contains(document.getElementById('attached-' + fieldId))) {
        alert('Поле уже добавлено.');
        return false;
    }

    var catalogId = $('#catalogId').val();
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '/Catalog/AttachField',
        type: 'Post',
        data: {
            __RequestVerificationToken: token,
            "catalogId": catalogId,
            "fieldId": fieldId
        },
        success: function (html) {
            $('#no-fields-message').remove();
            $('#attached-fields').append(html);
        },
        error: function (XmlHttpRequest) {
            alert('Ошибка. Обновите страницу.');
            console.log(XmlHttpRequest);
        }
    });
    return false;
}

function clearSearch() {
    $('#found-items').empty();
    $('#search-input-value').val('');
}