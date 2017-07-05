$(document).ready(function () {
    InitAsyncPostBack();
    InitDateTimeFunctions();
    InitFunctions();
});

function pageLoad() {
    var isAsyncPostback = Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();
    if (isAsyncPostback) {
        InitDateTimeFunctions();
        InitFunctions();
    }
};

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
            $get('UpdateProgressSearch').style.display = 'block';
            $get('UpdateProgressInvoice').style.display = 'block';
        }
        else if (postBackElement.id == 'ListBoxCompanies') {
            $get('UpdateProgressInvoice').style.display = 'block';
            $get('UpdateProgressTitle').style.display = 'block';
        }
        else if (postBackElement.id == 'CheckBoxDelivered') {
            $get('UpdateProgressInvoice').style.display = 'block';
        }
        else if (postBackElement.id == 'CheckBoxChecked') {
            $get('UpdateProgressInvoice').style.display = 'block';
        }
        else if (postBackElement.id == 'ButtonClearAccount') {
            $get('UpdateProgressInvoice').style.display = 'block';
            $get('UpdateProgressTitle').style.display = 'block';
        }
        else if (postBackElement.id == 'ButtonInsertAccount') {
            $get('UpdateProgressTitle').style.display = 'block';
        }
        else if (postBackElement.id == 'ButtonUpdateAccount') {
            $get('UpdateProgressTitle').style.display = 'block';
        }
        else if (postBackElement.id == 'ButtonDeleteAccount') {
            $get('UpdateProgressInvoice').style.display = 'block';
            $get('UpdateProgressTitle').style.display = 'block';
        }
        else if (postBackElement.id == 'ListBoxInvoices') {
            $get('UpdateProgressTitle').style.display = 'block';
        }
    }

    function EndRequest(sender, args) {
        if (postBackElement.id == 'ButtonSearch') {
            $get('UpdateProgressSearch').style.display = 'none';
            $get('UpdateProgressInvoice').style.display = 'none';
        }
        else if (postBackElement.id == 'ListBoxCompanies') {
            $get('UpdateProgressInvoice').style.display = 'none';
            $get('UpdateProgressTitle').style.display = 'none';
        }
        else if (postBackElement.id == 'CheckBoxDelivered') {
            $get('UpdateProgressInvoice').style.display = 'none';
        }
        else if (postBackElement.id == 'CheckBoxChecked') {
            $get('UpdateProgressInvoice').style.display = 'none';
        }
        else if (postBackElement.id == 'ButtonClearAccount') {
            $get('UpdateProgressInvoice').style.display = 'none';
            $get('UpdateProgressTitle').style.display = 'none';
        }
        else if (postBackElement.id == 'ButtonInsertAccount') {
            $get('UpdateProgressTitle').style.display = 'none';
        }
        else if (postBackElement.id == 'ButtonUpdateAccount') {
            $get('UpdateProgressTitle').style.display = 'none';
        }
        else if (postBackElement.id == 'ButtonDeleteAccount') {
            $get('UpdateProgressInvoice').style.display = 'none';
            $get('UpdateProgressTitle').style.display = 'none';
        }
        else if (postBackElement.id == 'ListBoxInvoices') {
            $get('UpdateProgressTitle').style.display = 'none';
        }
    }
};

function Round2(number, digits) {
    with (Math) {
        return round(number * pow(10, digits)) / pow(10, digits);
    }
}

function InitFunctions() {
    $('#TextBoxPayDays').change(function () {
        var textboxDays = $(this);
        var labelDate = $('#LabelDate');
        var labelPayDate = $('#LabelPayDate');

        var date = new Date(labelDate.html());
        date.setDate(date.getDate() + parseInt(textboxDays.val()));
        labelPayDate.html(date.format("yyyy/m/d"));
    });
    $('.textboxSize').live('change', function () {
        var textbox = $(this);

        var value = 0;
        try { value = parseInt(removeCommas(textbox.val())); }
        finally {
            if (isNaN(value)) textbox.val('0');
            else textbox.val(appendCommas(value.toFixed(0)));
        }

        row_amount(textbox);
    });
    $('.textboxPrice').live('change', function () {
        var textbox = $(this);

        var value = 0.00;
        try { value = parseFloat(removeCommas(textbox.val())); }
        finally {
            if (isNaN(value)) textbox.val('0.00');
            else textbox.val(appendCommas(Round2(value, 2)));
        }

        row_amount(textbox);
    });
    $('#TextBoxTax').change(function () {
        var textbox = $(this);

        var value = 0.00;
        try { value = parseFloat(removeCommas(textbox.val())); }
        finally {
            if (isNaN(value)) textbox.val('0.00');
            else textbox.val(appendCommas(Round2(value, 2)));
        }

        account_sum(false);
    });
    $('#TextBoxPrePay').change(function () {
        var textbox = $(this);

        var value = 0.00;
        try { value = parseFloat(removeCommas(textbox.val())); }
        finally {
            if (isNaN(value)) textbox.val('0.00');
            else textbox.val(appendCommas(Round2(value, 2)));
        }

        account_sum(false);
    });
};

function row_amount(textbox) {
    var rowIndex = textbox.context.tabIndex;

    var textboxSize0 = $('#ContentPlaceHolder1_GridViewAccount_TextBoxSize0_' + rowIndex);
    var textboxSize1 = $('#ContentPlaceHolder1_GridViewAccount_TextBoxSize1_' + rowIndex);
    var textboxSize2 = $('#ContentPlaceHolder1_GridViewAccount_TextBoxSize2_' + rowIndex);
    var textboxSize3 = $('#ContentPlaceHolder1_GridViewAccount_TextBoxSize3_' + rowIndex);
    var textboxSize4 = $('#ContentPlaceHolder1_GridViewAccount_TextBoxSize4_' + rowIndex);
    var textboxSize5 = $('#ContentPlaceHolder1_GridViewAccount_TextBoxSize5_' + rowIndex);
    var textboxSize6 = $('#ContentPlaceHolder1_GridViewAccount_TextBoxSize6_' + rowIndex);
    var textboxSize7 = $('#ContentPlaceHolder1_GridViewAccount_TextBoxSize7_' + rowIndex);
    var textboxSize8 = $('#ContentPlaceHolder1_GridViewAccount_TextBoxSize8_' + rowIndex);
    var textboxSize9 = $('#ContentPlaceHolder1_GridViewAccount_TextBoxSize9_' + rowIndex);
    var textboxSize10 = $('#ContentPlaceHolder1_GridViewAccount_TextBoxSize10_' + rowIndex);
    var textboxPrice = $('#ContentPlaceHolder1_GridViewAccount_TextBoxPrice_' + rowIndex);
    var labelQuantity = $('#ContentPlaceHolder1_GridViewAccount_LabelQuantity_' + rowIndex);
    var labelAmount = $('#ContentPlaceHolder1_GridViewAccount_LabelAmount_' + rowIndex);

    var size0 = parseInt(removeCommas(textboxSize0.val()));
    var size1 = parseInt(removeCommas(textboxSize1.val()));
    var size2 = parseInt(removeCommas(textboxSize2.val()));
    var size3 = parseInt(removeCommas(textboxSize3.val()));
    var size4 = parseInt(removeCommas(textboxSize4.val()));
    var size5 = parseInt(removeCommas(textboxSize5.val()));
    var size6 = parseInt(removeCommas(textboxSize6.val()));
    var size7 = parseInt(removeCommas(textboxSize7.val()));
    var size8 = parseInt(removeCommas(textboxSize8.val()));
    var size9 = parseInt(removeCommas(textboxSize9.val()));
    var size10 = parseInt(removeCommas(textboxSize10.val()));
    var price = parseFloat(removeCommas(textboxPrice.val()));

    var quantity = size0 + size1 + size2 + size3 + size4 + size5 + size6 + size7 + size8 + size9 + size10;
    labelQuantity.html(appendCommas(quantity));

    var amount = price * quantity;
    labelAmount.html(appendCommas(Round2(amount, 2)));

    account_sum(true);
};

function account_sum(culculate_tax) {
    var rows = $('#ContentPlaceHolder1_GridViewAccount tr').length - 1;
    if (rows <= 0) return;

    var labelTotalNum = $('#LabelTotalNum');
    var textboxTax = $('#TextBoxTax');
    var textboxPrePay = $('#TextBoxPrePay');
    var labelSum = $('#LabelSum');
    var labelTotal = $('#LabelTotal');
    var labelLeftPay = $('#LabelLeftPay');

    var sum = 0.0;
    var num = 0.0;
    for (var row = 0; row < rows; row++) {
        var label = $('#ContentPlaceHolder1_GridViewAccount_LabelAmount_' + row);
        var label1 = $('#ContentPlaceHolder1_GridViewAccount_LabelQuantity_' + row);
        sum += parseFloat(removeCommas(label.html()));
        num += parseFloat(removeCommas(label1.html()));
    }

    var tax = 0.00;
    if (culculate_tax) {
        tax = 0.05 * sum;
        textboxTax.val(appendCommas(Round2(tax, 2)));
    }
    else {
        tax = parseFloat(removeCommas(textboxTax.val()));
    }

    var prePay = parseFloat(removeCommas(textboxPrePay.val()));
    var total = sum + tax;
    var leftPay = total - prePay;

    labelTotalNum.html(appendCommas(Round2(num, 2)));
    labelSum.html(appendCommas(Round2(sum, 2)));
    labelTotal.html(appendCommas(Round2(total, 2)));
    labelLeftPay.html(appendCommas(Round2(leftPay, 2)));
};

function appendCommas(text) {
    text += '';

    var x = text.split('.');

    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;

    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }

    return x1 + x2;
}

function removeCommas(text) {
    return text.replace(',', '');
}

function InitDateTimeFunctions() {
    var dateFormat = function () {
        var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
		timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
		timezoneClip = /[^-+\dA-Z]/g,
		pad = function (val, len) {
		    val = String(val);
		    len = len || 2;
		    while (val.length < len) val = "0" + val;
		    return val;
		};

        // Regexes and supporting functions are cached through closure
        return function (date, mask, utc) {
            var dF = dateFormat;

            // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
            if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
                mask = date;
                date = undefined;
            }

            // Passing date through Date applies Date.parse, if necessary
            date = date ? new Date(date) : new Date;
            if (isNaN(date)) throw SyntaxError("invalid date");

            mask = String(dF.masks[mask] || mask || dF.masks["default"]);

            // Allow setting the utc argument via the mask
            if (mask.slice(0, 4) == "UTC:") {
                mask = mask.slice(4);
                utc = true;
            }

            var _ = utc ? "getUTC" : "get",
			d = date[_ + "Date"](),
			D = date[_ + "Day"](),
			m = date[_ + "Month"](),
			y = date[_ + "FullYear"](),
			H = date[_ + "Hours"](),
			M = date[_ + "Minutes"](),
			s = date[_ + "Seconds"](),
			L = date[_ + "Milliseconds"](),
			o = utc ? 0 : date.getTimezoneOffset(),
			flags = {
			    d: d,
			    dd: pad(d),
			    ddd: dF.i18n.dayNames[D],
			    dddd: dF.i18n.dayNames[D + 7],
			    m: m + 1,
			    mm: pad(m + 1),
			    mmm: dF.i18n.monthNames[m],
			    mmmm: dF.i18n.monthNames[m + 12],
			    yy: String(y).slice(2),
			    yyyy: y,
			    h: H % 12 || 12,
			    hh: pad(H % 12 || 12),
			    H: H,
			    HH: pad(H),
			    M: M,
			    MM: pad(M),
			    s: s,
			    ss: pad(s),
			    l: pad(L, 3),
			    L: pad(L > 99 ? Math.round(L / 10) : L),
			    t: H < 12 ? "a" : "p",
			    tt: H < 12 ? "am" : "pm",
			    T: H < 12 ? "A" : "P",
			    TT: H < 12 ? "AM" : "PM",
			    Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
			    o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
			    S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
			};

            return mask.replace(token, function ($0) {
                return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
            });
        };
    } ();

    // Some common format strings
    dateFormat.masks = {
        "default": "ddd mmm dd yyyy HH:MM:ss",
        shortDate: "m/d/yy",
        mediumDate: "mmm d, yyyy",
        longDate: "mmmm d, yyyy",
        fullDate: "dddd, mmmm d, yyyy",
        shortTime: "h:MM TT",
        mediumTime: "h:MM:ss TT",
        longTime: "h:MM:ss TT Z",
        isoDate: "yyyy-mm-dd",
        isoTime: "HH:MM:ss",
        isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
        isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
    };

    // Internationalization strings
    dateFormat.i18n = {
        dayNames: [
		"Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
		"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
	],
        monthNames: [
		"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
		"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
	]
    };

    // For convenience...
    Date.prototype.format = function (mask, utc) {
        return dateFormat(this, mask, utc);
    };
};

function OnSearch(button, event) {
    var key;
    if (window.event)
        key = window.event.keyCode; // IE
    else
        key = e.which; // Firefox

    if (key == 13) {
        event.keyCode = 0
        button.click();
    }
};