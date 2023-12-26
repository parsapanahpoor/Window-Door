$("#MainCategory").change(function () {
    if ($("#MainCategory :selected").val() !== '') {
        $('#FirstSubCategory option:not(:first)').remove();
        $.get("/Seller/ShopProduct/LoadSubCategories", { MainCategoryId: $("#MainCategory :selected").val() }).then(res => {
            if (res.data !== null) {
                $.each(res.data, function () {
                    $("#FirstSubCategory").append(
                        '<option value=' + this.id + '>' + this.title + '</option>'
                    );
                });
            }
        });
    } else {
        $('#FirstSubCategory option:not(:first)').remove();
    }
});

$("#FirstSubCategory").change(function () {
    if ($("#FirstSubCategory :selected").val() !== '') {
        $('#SecondeSubCategory option:not(:first)').remove();
        $.get("/Seller/ShopProduct/LoadSubCategories", { MainCategoryId: $("#FirstSubCategory :selected").val() }).then(res => {
            if (res.data !== null) {
                $.each(res.data, function () {
                    $("#SecondeSubCategory").append(
                        '<option value=' + this.id + '>' + this.title + '</option>'
                    );
                });
            }
        });
    } else {
        $('#SecondeSubCategory option:not(:first)').remove();
    }
});