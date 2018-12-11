function searchFields() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var field = $('#search-input-value').val();
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
            console.log(XmlHttpRequest.responseText);
        }
    });
    return false;
}

function attachField(fieldId) {
    if (document.body.contains(document.getElementById('attached-' + fieldId))) {
        alert('Поле уже добавлено.');
        return false;
    }

    var token = $('input[name="__RequestVerificationToken"]').val();
    var catalogId = $('#catalogId').val();

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
            console.log(XmlHttpRequest.responseText);
        }
    });
    return false;
}

function detachField(fieldId) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var catalogId = $('#catalogId').val();

    $.ajax({
        url: '/Catalog/DetachField',
        type: 'Post',
        data: {
            __RequestVerificationToken: token,
            'catalogId': catalogId,
            'fieldId': fieldId
        },
        success: function (message) {
            if (message === 'success') {
                $('#attached-' + fieldId).remove();
            } else if (message === 'fail') {
                alert('Ошибка. Обновите страницу и попробуйте еще раз.');
            }
        },
        error: function (XmlHttpRequest) {
            alert('Ошибка. Обновите страницу и поробуйте еще раз.');
            console.log(XmlHttpRequest.responseText);
        }
    });
    return false;
}

function clearSearch() {
    $('#found-items').empty();
    $('#search-input-value').val('');
}

function addRow() {
    if ($('.row-form').length > 0) {
        alert('Сохраните открытую форму');
        return false;
    }

    var catalogId = $('#catalogId').val();
    var token = $('input[name="__RequestVerificationToken"]').val();

    var rowNumber;
    if ($('.row-number').length > 0) {
        rowNumber = parseInt($('.row-number').last().text()) + 1;
    }
    else {
        rowNumber = 1;
    }

    $.ajax({
        url: '/Catalog/AddRow',
        type: 'Post',
        data: {
            __RequestVerificationToken: token,
            'catalogId': catalogId,
            'rowNumber': rowNumber
        },
        success: function (form) {
            $('#add-row-button').hide();
            $('#create-row').append(form);
        },
        error: function (XmlHttpRequest) {
            alert('Ошибка. Обновите страницу и попробуйте снова.');
            console.log(XmlHttpRequest.responseText);
        }
    });
    return false;
}

function save() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var catalogId = $('#catalogId').val();
    var rowNubmer = $('#fieldRowNumber').val();
    var values = [];

    $('.add-row-form-field').each(function () {
        values.push({
            "Title": $(this).find(".field-value").first().val(),
            "Row": rowNubmer,
            "FieldId": $(this).find(".field-id").first().val(),
            "CatalogId": catalogId
        });
    });

    $.ajax({
        url: '/Catalog/AddedRow',
        type: 'Post',
        data: {
            __RequestVerificationToken: token,
            'values': values
        },
        success: function (html) {
            $('#add-row-form').remove();
            $('#add-row-button').show();
            $('#catalog-values').append(html);
        },
        error: function (XmlHttpRequest) {
            alert('Ошибка. Обновите страницу и попробуйте снова.');
            console.log(XmlHttpRequest.responseText);
        }
    });
    return false;
}

function editRow(rowNumber) {
    if ($('.row-form').length > 0) {
        alert('Сохраните открытую форму');
        return false;
    }

    var catalogId = $('#catalogId').val();
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '/Catalog/EditRow',
        type: 'Post',
        data: {
            __RequestVerificationToken: token,
            'catalogId': catalogId,
            'rowNumber': rowNumber
        },
        success: function (form) {
            $('#row-' + rowNumber).html(form);
        },
        error: function (XmlHttpRequest) {
            alert('Ошибка. Обновите страницу и попробуйте еще раз.');
            console.log(XmlHttpRequest.responseText);
        }
    });
    return false;
}

function saveChanges() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var catalogId = $('#catalogId').val();
    var rowNumber = $('#fieldRowNumber').val();
    var values = [];

    $('.edit-row-form-field').each(function () {
        values.push({
            "Id": $(this).find(".value-id").first().val(),
            "Title": $(this).find(".field-value").first().val(),
            "Row": rowNumber,
            "FieldId": $(this).find(".field-id").first().val(),
            "CatalogId": catalogId
        });
    });

    $.ajax({
        url: '/Catalog/EditedRow',
        type: 'Post',
        data: {
            __RequestVerificationToken: token,
            'values': values
        },
        success: function (html) {
            $('#edit-row-form').remove();
            $('#row-' + rowNumber).html(html);
        },
        error: function (XmlHttpRequest) {
            alert('Ошибка. Обновите страницу и попробуйте снова.');
            console.log(XmlHttpRequest.responseText);
        }
    });
    return false;
}

function removeRow(rowNumber) {
    var confirmation = confirm('Вы уверены, что хотите удалить строку?');
    if (confirmation) {
        var catalogId = $('#catalogId').val();

        $.ajax({
            url: '/Catalog/DeleteRow',
            type: 'Post',
            data: {
                __RequestVerificationToken: token,
                'catalogId': catalogId,
                'rowNumber': rowNumber
            },
            success: function () {
                $('#row-' + rowNumber).remove();
            },
            error: function (XmlHttpRequest) {
                alert('Ошибка. Обновите страницу и попробуйте еще раз.');
                console.log(XmlHttpRequest.responseText);
            }
        });
        return false;
    }
}

function confirmDelete() {
    return confirm('Вы уверены, что хотите удалить строку?');
}