$(document).ready(function () {
    var $rows = $("#ContentPlaceHolder1_GridViewReport > tbody > tr");
    if (null == $rows) return;

    var index = 0;
    var height = 0;
    $rows.each(function (i) {
        if (i == $rows.length - 1) return;

        height += $(this).height();

        if (0 == index) {
            if (height >= 1050) {
                $(this).after($("#HeaderRow").clone().css('page-break-before', 'always'));
                height = 0;
                index++;
            }
        }
        else {
            if (height >= 1100) {
                $(this).after($("#HeaderRow").clone().css('page-break-before', 'always'));
                height = 0;
                index++;
            }
        }
    });
});
