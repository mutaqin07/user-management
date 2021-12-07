
function SubmitForm(title, text, confirmatinButtonText, cancelButtonText, url, formdata, OkHandler) {
    var baseUrl = $('base').attr('href');

    Swal.fire({
        title: title,
        text: text,
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: confirmatinButtonText,
        cancelButtonText: cancelButtonText
    }).then(function (result) {
        if (result.value) {
            KTApp.blockPage({
                overlayColor: '#000000',
                state: 'danger',
                message: 'Harap tunggu...'
            });
           
            fetch(baseUrl + url, {
                method: "POST",                   
                body: formdata,               
                headers: { "ClientScript": true, "X-ANTI-FORGERY-TOKEN": document.getElementsByName("__RequestVerificationToken")[0].value }
            }).then(async response => {
                KTApp.unblockPage();
                console.log(response);
                switch (response.status) {
                    case 200:
                        var responseJson = await response.json();
                       
                        document.getElementsByName("__RequestVerificationToken")[0].value = responseJson.newAntiForgeryToken;
                       
                        if (responseJson.status == "OK") {
                            OkHandler();
                        }
                        break;
                    case 401:
                        var result = await response.json();

                        switch (result.message) {
                            case "NoAccessRight":
                                document.getElementsByName("__RequestVerificationToken")[0].value = result.newAntiForgeryToken;
                                Swal.fire("Error", "Maaf, anda tidak memilik hak menggunakan modul ini...!", "error");
                                break;
                            case "CookiesExpired":
                                window.location.href = baseUrl + 'Account/Login';                              
                                break;
                            default:
                                Swal.fire("Error : 401", response.statusText, "error");
                                break;
                        }                      
                        break;                   
                    default:
                        Swal.fire("Error : " + response.status, response.statusText, "error");
                        break;
                }
            }).catch( err => {               
                KTApp.unblockPage();
                var errorString = JSON.stringify(err);                

                swal.fire({
                    text: errorString,
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
        }
    });  
};