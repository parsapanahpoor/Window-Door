function ShowMessage(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'nfc-bottom-right',
        showDuration: 4000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'اعلان',
        message: decodeURI(text)
    });
}

function FillPageId(pageId) {
    $('#PageId').val(pageId);
    $('#filter-form').submit();
}


//$(document).ready(function () {
//    var editors = $("[ckeditor]");
//    if (editors.length > 0) {
//        $.getScript('/js/ckeditor.js', function () {
//            $(editors).each(function (index, value) {
//                var id = $(value).attr('ckeditor');
//                ClassicEditor.create(document.querySelector('[ckeditor="' + id + '"]'),
//                    {
//                        toolbar: {
//                            items: [
//                                'heading',
//                                '|',
//                                'bold',
//                                'italic',
//                                'link',
//                                '|',
//                                'fontSize',
//                                'fontColor',
//                                '|',
//                                'imageUpload',
//                                'blockQuote',
//                                'insertTable',
//                                'undo',
//                                'redo',
//                                'codeBlock'
//                            ]
//                        },
//                        language: 'fa',
//                        table: {
//                            contentToolbar: [
//                                'tableColumn',
//                                'tableRow',
//                                'mergeTableCells'
//                            ]
//                        },
//                        licenseKey: '',
//                        simpleUpload: {
//                            // The URL that the images are uploaded to.
//                            uploadUrl: '/Uploader/UploadImage'
//                        }

//                    })
//                    .then(editor => {
//                        window.editor = editor;
//                    }).catch(err => {
//                        console.error(err);
//                    });
//            });
//        });
//    }
//});


function FillPageId(pageId) {
    $("#PageId").val(pageId);
    $("#filter-form").submit();
}

$('[ajax-url-button]').on('click', function (e) {
    e.preventDefault();
    var itemUrl = $(this).attr('href');
    var itemId = $(this).attr('ajax-url-button');
    swal({
        title: 'اخطار',
        text: "آیا از انجام عملیات مورد نظر اطمینان دارید؟",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "بله",
        cancelButtonText: "خیر",
        closeOnConfirm: false,
        closeOnCancel: false
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: itemUrl,
                type: 'get',
                data: null,
                contentType: false,
                processData: false,
                success: function (response) {
                    //ShowMessage('موفقیت', result.message);
                    //        //$('#ajax-url-item-' + itemId).hide(1500);
                    location.reload();
                }
            });
            
            //$.get(itemUrl).done(function(result) {
            //    if (result.status.toLocaleString() === 'success') {
            //        //ShowMessage('موفقیت', result.message);
            //        //$('#ajax-url-item-' + itemId).hide(1500);
            //        location.reload();
            //    }
            //});
        } else if (result.dismiss === swal.DismissReason.cancel) {
            swal('اعلام', 'عملیات لغو شد', 'error');
        }
    });
});

function OnSuccessRejectItem(res) {
    if (res.status === 'Success') {
        ShowMessage('اعلان موفقیت', res.message);
        $('#ajax-url-item-' + res.data.id).hide(300);
        $('#reject-modal-' + res.data.id).modal('toggle');
        $('#reject-modal-' + res.data.id).modal().hide();
        $('.close').click();
    }
}


//$('[ajax-url-button]').on('click', function (e) {
//    e.preventDefault();
//    var url = $(this).attr('href');
//    var itemId = $(this).attr('ajax-url-button');
//    swal({
//        title: 'اخطار',
//        text: "آیا از انجام عملیات مورد نظر اطمینان دارید؟",
//        type: "warning",
//        showCancelButton: true,
//        confirmButtonClass: "btn-danger",
//        confirmButtonText: "بله",
//        cancelButtonText: "خیر",
//        closeOnConfirm: false,
//        closeOnCancel: false
//    }).then((result) => {
//        if (result.value) {
//            $.get(url).then(result => {
//                if (result.status === 'Success') {
//                    ShowMessage('موفقیت', result.message);
//                    if (result.data === null) {
//                        $('#ajax-url-item-' + itemId).hide(1500);
//                    } else {
//                        if (result.data === 'ActiveUser') {
//                            $("#Actived_" + itemId).removeClass("badge light badge-success cursor-pointer").addClass("badge light badge-danger cursor-pointer").css("color", "#f55")
//                                .html('غیرفعال').attr('id', "DisActived_" + itemId).attr('href', '/Admin/Users/DisActiveUsers?userId=' + itemId);

//                        } else if (result.data === 'DisActiveUser') {
//                            $("#DisActived_" + itemId).removeClass("badge light badge-danger cursor-pointer").addClass("badge light badge-success cursor-pointer").css("color", "#00bf00")
//                                .html('فعال').attr('id', "Actived_" + itemId).attr('href', '/Admin/Users/ActiveUsers?userId=' + itemId);
//                        }
//                    }

//                }
//            });
//        } else if (result.dismiss === swal.DismissReason.cancel) {
//            swal('اعلام', 'عملیات لغو شد', 'error');
//        }
//    });
//});

function OnSuccessRejectItem(res) {
    if (res.status === 'Success') {
        ShowMessage('اعلان موفقیت', res.message);
        $('#ajax-url-item-' + res.data.id).hide(300);
        $('#reject-modal-' + res.data.id).modal('toggle');
        $('#reject-modal-' + res.data.id).modal().hide();
        $('.close').click();
    }
}

$("[Preview-Advertisement]").on("click",
    function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        if (url != null && url != "" && url != undefined) {
            $.get(url).then(function (res) {
                if (res != null && res != undefined && res != "") {
                    $("#preview-advertisement-modal-body iframe").attr("srcdoc", res);
                    $("#preview-advertisement-modal").modal();
                }
            });
        }
    });