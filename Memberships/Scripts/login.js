$(function() {

    $("#loginlink").hover(onLoginLinkHover);
    $("#close-login").click(onLoginClosePanel);

    function onLoginLinkHover() {
        $("div[data-login-user-area]").addClass('open');
    }
    function onLoginClosePanel() {
        $("div[data-login-user-area]").removeClass('open');
    }
});