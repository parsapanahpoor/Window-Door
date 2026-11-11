function LoadAddPersonalLinkModalBody() {
    $.ajax({
        url: "/Show-AddPErsonalLink-Modal",
        type: "get",
        success: function (response) {
            $("#NormalModalBody").html(response);
            $("#NormalModalTitle").text("ثبت لینک ارتباطی");
            $("#PersonalLinkForm").data("validator", null);
            $.validator.unobtrusive.parse("#PersonalLinkForm");
            $("#NormalModal").modal("show");
        },
        error: function () {
            ShowMessage("خطا", "فرم لینک ارتباطی بارگذاری نشد.", "error");
        }
    });
}

function PersonalLinkFormSubmited(response) {
    if (response.status === "Success") {
        $("#NormalModal").modal("hide");
        ShowMessage("اعلان", "عملیات با موفقیت انجام شده است", "success");
        location.reload();
    } else {
        ShowMessage("اعلان", "عملیات با شکست مواجه شد", "error");
    }
}

function LoadAddPersonalWorkSampleModalBody() {
    $.ajax({
        url: "/Show-AddPersonalWorkSample-Modal",
        type: "get",
        success: function (response) {
            $("#NormalModalBody").html(response);
            $("#NormalModalTitle").text("افزودن نمونه‌کار");
            $("#PersonalWorkSampleForm").data("validator", null);
            $.validator.unobtrusive.parse("#PersonalWorkSampleForm");
            $("#NormalModal").modal("show");

            $("#image").off("change.sellerWorkSample").on("change.sellerWorkSample", function () {
                const input = this;
                const file = input.files && input.files[0];
                if (!file) return;

                const extension = file.name.split(".").pop().toLowerCase();
                if (!["png", "jpg", "jpeg"].includes(extension)) {
                    input.value = "";
                    $("#Avatar").val("");
                    $("#SellerWorkSampleUploadStatus").text("فقط تصویر JPG یا PNG مجاز است.");
                    ShowMessage("خطا", "فرمت فایل باید JPG یا PNG باشد.", "error");
                    return;
                }

                const preview = document.getElementById("SellerWorkSamplePreview");
                const placeholder = document.getElementById("SellerWorkSamplePlaceholder");
                if (preview.dataset.objectUrl) URL.revokeObjectURL(preview.dataset.objectUrl);
                preview.dataset.objectUrl = URL.createObjectURL(file);
                preview.src = preview.dataset.objectUrl;
                preview.hidden = false;
                placeholder.hidden = true;

                const formData = new FormData();
                formData.append("file", file);
                const token = $("#PersonalWorkSampleForm input[name=__RequestVerificationToken]").val();
                if (token) formData.append("__RequestVerificationToken", token);
                const submit = $("#PersonalWorkSampleForm button[type=submit]");
                submit.prop("disabled", true);
                $("#SellerWorkSampleUploadStatus").text("در حال بارگذاری تصویر…");

                $.ajax({
                    type: "POST",
                    url: "/Seller/SellerPersonalInfo/UploadSellerWorkSampleImgeInModal",
                    data: formData,
                    cache: false,
                    contentType: false,
                    processData: false,
                    success: function (result) {
                        if (result.status === "Success") {
                            $("#Avatar").val(result.imageName);
                            $("#SellerWorkSampleUploadStatus").text("تصویر آمادهٔ ثبت است.");
                        } else {
                            $("#Avatar").val("");
                            $("#SellerWorkSampleUploadStatus").text("بارگذاری تصویر انجام نشد. دوباره تلاش کنید.");
                        }
                    },
                    error: function () {
                        $("#Avatar").val("");
                        $("#SellerWorkSampleUploadStatus").text("بارگذاری تصویر انجام نشد. دوباره تلاش کنید.");
                        ShowMessage("خطا", "بارگذاری تصویر انجام نشد.", "error");
                    },
                    complete: function () {
                        submit.prop("disabled", false);
                    }
                });
            });
        },
        error: function () {
            ShowMessage("خطا", "فرم نمونه‌کار بارگذاری نشد.", "error");
        }
    });
}

function PersonalWorkSampleFormSubmited(response) {
    if (response.status === "Success") {
        $("#NormalModal").modal("hide");
        ShowMessage("اعلان", "نمونه‌کار با موفقیت ثبت شد.", "success");
        location.reload();
    } else {
        ShowMessage("خطا", "ثبت نمونه‌کار انجام نشد. عنوان و تصویر را بررسی کنید.", "error");
    }
}