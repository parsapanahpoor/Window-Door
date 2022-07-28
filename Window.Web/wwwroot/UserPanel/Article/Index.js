const mainCategoryEl = document.getElementById('MainCategoryId');
const subCategoryEl = document.getElementById('SubCategoryId');

$(mainCategoryEl).on('change', function(event) {
    $(subCategoryEl).attr('disabled', '');

    const mainCategoryId = $(mainCategoryEl).val();

    $.ajax({
        url: "/User/Article/GetSubCategories/" + mainCategoryId,
        method: "get",
        success: function(response) {
            insertData(response.data, subCategoryEl);
            $(subCategoryEl).removeAttr('disabled');
        }
    });

});

function insertData(data, targetEl) {

    var baseOption = $(targetEl).children()[0];
    $(targetEl).html('');
    $(targetEl).append(baseOption);

    for (let category of data) {
        var newOption = document.createElement('option');
        newOption.setAttribute('value', category.id);
        newOption.innerHTML = category.title;

        $(targetEl).append(newOption);
    }
}