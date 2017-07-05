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
        if (postBackElement.id == 'ButtonReport')
            $get('UpdateProgressReport').style.display = 'block';
    }

    function EndRequest(sender, args) {
        if (postBackElement.id == 'ButtonReport')
            $get('UpdateProgressReport').style.display = 'none';
    }
};
