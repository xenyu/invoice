$(document).ready(function () {
    InitAsyncPostBack();
});

function InitAsyncPostBack() {
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequest);
    prm.add_endRequest(EndRequest);

    var postBackElement;

    function InitializeRequest(sender, args) {
        if (prm.get_isInAsyncPostBack())
            args.set_cancel(true);

        postBackElement = args.get_postBackElement();
        if (postBackElement.id == 'ButtonSearch') {
            $get('UpdateProgressData').style.display = 'block';
            $get('UpdateProgressSearch').style.display = 'block';
            $get('UpdateProgressRecord').style.display = 'block';
        }
        else if (postBackElement.id == 'ListBoxCompanies') {
            $get('UpdateProgressData').style.display = 'block';
            $get('UpdateProgressRecord').style.display = 'block';
        }
        else if (postBackElement.id == 'MenuCompany') {
            $get('UpdateProgressData').style.display = 'block';
            $get('UpdateProgressRecord').style.display = 'block';
        }
        else if (postBackElement.id == 'ButtonClear') {
            $get('UpdateProgressData').style.display = 'block';
            $get('UpdateProgressSearch').style.display = 'block';
            $get('UpdateProgressRecord').style.display = 'block';
        }
        else if (postBackElement.id == 'ButtonInsert') {
            $get('UpdateProgressData').style.display = 'block';
            $get('UpdateProgressSearch').style.display = 'block';
            $get('UpdateProgressRecord').style.display = 'block';
        }
        else if (postBackElement.id == 'ButtonUpdate') {
            $get('UpdateProgressData').style.display = 'block';
            $get('UpdateProgressSearch').style.display = 'block';
        }
        else if (postBackElement.id == 'ButtonDelete') {
            $get('UpdateProgressData').style.display = 'block';
            $get('UpdateProgressSearch').style.display = 'block';
            $get('UpdateProgressRecord').style.display = 'block';
        }
    }

    function EndRequest(sender, args) {
        if (postBackElement.id == 'ButtonSearch') {
            $get('UpdateProgressData').style.display = 'none';
            $get('UpdateProgressSearch').style.display = 'none';
            $get('UpdateProgressRecord').style.display = 'none';
        }
        else if (postBackElement.id == 'ListBoxCompanies') {
            $get('UpdateProgressData').style.display = 'none';
            $get('UpdateProgressRecord').style.display = 'none';
        }
        else if (postBackElement.id == 'MenuCompany') {
            $get('UpdateProgressData').style.display = 'none';
            $get('UpdateProgressRecord').style.display = 'none';
        }
        else if (postBackElement.id == 'ButtonClear') {
            $get('UpdateProgressData').style.display = 'none';
            $get('UpdateProgressSearch').style.display = 'none';
            $get('UpdateProgressRecord').style.display = 'none';
        }
        else if (postBackElement.id == 'ButtonInsert') {
            $get('UpdateProgressData').style.display = 'none';
            $get('UpdateProgressSearch').style.display = 'none';
            $get('UpdateProgressRecord').style.display = 'none';
        }
        else if (postBackElement.id == 'ButtonUpdate') {
            $get('UpdateProgressData').style.display = 'none';
            $get('UpdateProgressSearch').style.display = 'none';
        }
        else if (postBackElement.id == 'ButtonDelete') {
            $get('UpdateProgressData').style.display = 'none';
            $get('UpdateProgressSearch').style.display = 'none';
            $get('UpdateProgressRecord').style.display = 'none';
        }
    }
}

function OnSearch(button, event) {
    var key;
    if (window.event)
        key = window.event.keyCode; // IE
    else
        key = e.which; // Firefox

    if (key == 13) {
        event.keyCode = 0;
        button.click();
    }
};