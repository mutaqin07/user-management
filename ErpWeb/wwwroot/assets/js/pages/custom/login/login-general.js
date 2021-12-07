"use strict";

var baseUrl = $('base').attr('href');

// Class Definition
var KTLogin = function () {
    var _login;

    var _showForm = function (form) {
        var cls = 'login-' + form + '-on';
        var form = 'kt_login_' + form + '_form';

        _login.removeClass('login-forgot-on');
        _login.removeClass('login-signin-on');
        /* _login.removeClass('login-signup-on');*/

        _login.addClass(cls);

        KTUtil.animateClass(KTUtil.getById(form), 'animate__animated animate__backInUp');
    }

    var _handleSignInForm = function () {
        var validation;

        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validation = FormValidation.formValidation(
            KTUtil.getById('kt_login_signin_form'),
            {
                fields: {
                    username: {
                        validators: {
                            notEmpty: {
                                message: 'Tidak boleh kosong'
                            }
                        }
                    },
                    password: {
                        validators: {
                            notEmpty: {
                                message: 'Tidak boleh kosong'
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    submitButton: new FormValidation.plugins.SubmitButton(),
                    //defaultSubmit: new FormValidation.plugins.DefaultSubmit(), // Uncomment this line to enable normal button submit after form validation
                    bootstrap: new FormValidation.plugins.Bootstrap()
                }
            }
        );

        $('#kt_login_signin_submit').on('click', function (e) {
            e.preventDefault();

            KTApp.blockPage({
                overlayColor: '#000000',
                state: 'danger',
                message: 'Harap tunggu...'
            });

            validation.validate().then(function (status) {
                if (status == 'Valid') {
                    const queryString = window.location.search;
                    const urlParams = new URLSearchParams(queryString);
                    const urlReturn = urlParams.get('ReturnUrl');

                    var loginData = {
                        'UserName': $('#txtUserName').val(),
                        'Password': $('#txtPassword').val(),
                        'RememberMe': true,
                        'ReturnUrl': urlReturn
                    }

                    fetch(baseUrl + 'Account/Login', {
                        method: "POST",
                        body: JSON.stringify(loginData),
                        headers: { "Content-type": "application/json; charset=UTF-8" }
                    })
                        .then(async response => {
                            var loginResult = await response.json();

                            KTApp.unblockPage();

                            switch (loginResult.status) {
                                case "OK":
                                    if (loginResult.returnUrl == null || loginResult.returnUrl == '') {
                                        window.location.href = baseUrl + 'MainMenu/Index';
                                    }
                                    else if (loginResult.returnUrl == '/Error/404') {
                                        window.location.href = baseUrl + 'MainMenu/Index';
                                    }
                                    else {
                                        window.location.href = baseUrl + loginResult.returnUrl.slice(1);
                                    }

                                    break;
                                case "UnAuthorize":
                                    swal.fire({
                                        text: "NIM atau kata sandi yang anda masukkan salah...!",
                                        icon: "error",
                                        buttonsStyling: false,
                                        confirmButtonText: "Ok",
                                        customClass: {
                                            confirmButton: "btn font-weight-bold btn-light-primary"
                                        }
                                    }).then(function () {
                                        KTUtil.scrollTop();
                                    });
                                    break;
                                default:

                            }
                         
                        }).catch(err => {
                            KTApp.unblockPage();
                            swal.fire({
                                text: err,
                                icon: "error",
                                buttonsStyling: false,
                                confirmButtonText: "Ok",
                                customClass: {
                                    confirmButton: "btn font-weight-bold btn-light-primary"
                                }
                            }).then(function () {
                                KTUtil.scrollTop();
                            })
                        });

                } else {
                    KTApp.unblockPage();
                    swal.fire({
                        text: "Maaf, NIK / NIM dan Password tidak boleh kosong...!",
                        icon: "error",
                        buttonsStyling: false,
                        confirmButtonText: "Ok",
                        customClass: {
                            confirmButton: "btn font-weight-bold btn-light-primary"
                        }
                    }).then(function () {
                        KTUtil.scrollTop();
                    });
                }
            });
        });

        // Handle forgot button
        $('#kt_login_forgot').on('click', function (e) {
            e.preventDefault();
            _showForm('forgot');
        });
    }

    var _handleForgotForm = function (e) {
        var validation;

        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validation = FormValidation.formValidation(
            KTUtil.getById('kt_login_forgot_form'),
            {
                fields: {
                    email: {
                        validators: {
                            notEmpty: {
                                message: 'Tidak boleh kosong'
                            },
                            emailAddress: {
                                message: 'Format email salah'
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    bootstrap: new FormValidation.plugins.Bootstrap()
                }
            }
        );

        // Handle submit button
        $('#kt_login_forgot_submit').on('click', function (e) {
            e.preventDefault();

            validation.validate().then(function (status) {
                if (status == 'Valid') {
                    // Submit form
                    KTUtil.scrollTop();
                } else {
                    swal.fire({
                        text: "Email tidak boleh kosong atau format email salah",
                        icon: "error",
                        buttonsStyling: false,
                        confirmButtonText: "Ok",
                        customClass: {
                            confirmButton: "btn font-weight-bold btn-light-primary"
                        }
                    }).then(function () {
                        KTUtil.scrollTop();
                    });
                }
            });
        });

        // Handle cancel button
        $('#kt_login_forgot_cancel').on('click', function (e) {
            e.preventDefault();

            _showForm('signin');
        });
    }

    // Public Functions
    return {
        // public functions
        init: function () {
            _login = $('#kt_login');

            _handleSignInForm();
            /* _handleSignUpForm();*/
            _handleForgotForm();
        }
    };
}();

// Class Initialization
jQuery(document).ready(function () {
    KTLogin.init();
});
