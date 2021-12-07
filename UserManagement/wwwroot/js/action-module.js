const token = document.getElementsByName("__RequestVerificationToken")[0].value;
const baseUrl = $('base').attr('href');


const GetById = async (urlParam, id) => {
    KTApp.blockPage({
        overlayColor: 'grey',
        opacity: 0.1,
        state: 'primary'
    });

    let params = {
        "id": id,
    };
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');
    let url = `${urlParam}?` + query;

    let options = {
        method: 'POST',
        headers: {
            "X-ANTI-FORGERY-TOKEN": token,
        }
    };

    try {
        const response = await fetch(url, options);
        var message;
        var responseJson = await response.json();
        if (!response.ok) {
            switch (responseJson.MessageStatus) {
                case "NoAccessRight":
                    message = "Maaf, anda tidak memilik hak menggunakan modul ini...!";
                    throw new Error(message);
                    break;
                case "CookiesExpired":
                    window.location.href = baseUrl + '/Account/Login';
                    break;
                case "AntiForgeryTokenExpired":
                    window.location.href = baseUrl + '/Account/Login';
                    break;
                default:
                    message = 'Error : ' + responseJson.Message;
                    throw new Error(message);
                    break;
            }
        }
        return responseJson;

    } catch (error) {
        Swal.fire("Error", error.message, "error");
    } finally {
        KTApp.unblockPage();
    }
};

const GetDataByParam = async (urlParam, params) => {
    KTApp.blockPage({
        overlayColor: 'grey',
        opacity: 0.1,
        state: 'primary'
    });

    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');
    let url = `${urlParam}?` + query;

    let options = {
        method: 'POST',
        headers: {
            "X-ANTI-FORGERY-TOKEN": token,
        }
    };

    try {
        const response = await fetch(url, options);
        var message;
        var responseJson = await response.json();
        if (!response.ok) {
            switch (responseJson.MessageStatus) {
                case "NoAccessRight":
                    message = "Maaf, anda tidak memilik hak menggunakan modul ini...!";
                    throw new Error(message);
                    break;
                case "CookiesExpired":
                    window.location.href = baseUrl + '/Account/Login';
                    break;
                case "AntiForgeryTokenExpired":
                    window.location.href = baseUrl + '/Account/Login';
                    break;
                default:
                    message = 'Error : ' + responseJson.Message;
                    throw new Error(message);
                    break;
            }
        }
        return responseJson;

    } catch (error) {
        Swal.fire("Error", error.message, "error");
    } finally {
        KTApp.unblockPage();
    }
};

const SubmitForm = async (urlParam, formData) => {
    KTApp.blockPage({
        overlayColor: 'grey',
        opacity: 0.1,
        state: 'primary'
    });

    let options = {
        method: 'POST',
        headers: {
            "X-ANTI-FORGERY-TOKEN": token,
        },
        body: formData
    };

    try {
        const response = await fetch(urlParam, options);

        var message;
        var responseJson = await response.json();
        
        switch (responseJson.MessageStatus) {
            case "NoAccessRight":
                message = "Maaf, anda tidak memilik hak menggunakan modul ini...!";
                throw new Error(message);
                break;
            case "CookiesExpired":
                window.location.href = baseUrl + '/Account/Login';
                break;
            case "AntiForgeryTokenExpired":
                window.location.href = baseUrl + '/Account/Login';
                break;
            case "OK":
                return responseJson.data;
                break;
            default:
                message = 'Error : ' + responseJson.Message;
                throw new Error(message);
                break;
        }             
    } catch (error) {
        Swal.fire("Error", error.message, "error");
    } finally {
        KTApp.unblockPage();
    }
};

const DeleteById = async (urlParam, id) => {
    KTApp.blockPage({
        overlayColor: 'grey',
        opacity: 0.1,
        state: 'primary'
    });

    let params = {
        "id": id,
    };
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');
    let url = `${urlParam}?` + query;

    let options = {
        method: 'POST',
        headers: {
            "X-ANTI-FORGERY-TOKEN": token,
        }
    };

    try {
        const response = await fetch(url, options);
        if (!response.ok) {
            var message;
            var responseJson = await response.json();
            switch (responseJson.MessageStatus) {
                case "NoAccessRight":
                    message = "Maaf, anda tidak memilik hak menggunakan modul ini...!";
                    throw new Error(message);
                    break;
                case "CookiesExpired":
                    window.location.href = baseUrl + '/Account/Login';
                    break;
                case "AntiForgeryTokenExpired":
                    window.location.href = baseUrl + '/Account/Login';
                    break;
                default:
                    message = 'Error : ' + responseJson.Message;
                    throw new Error(message);
                    break;
            }
        } else {
            return response;
        }
        
    } catch (error) {
        Swal.fire("Error", error.message, "error");
    } finally {
        KTApp.unblockPage();
    }
}

const DeleteByParam = async (urlParam, params) => {
    KTApp.blockPage({
        overlayColor: 'grey',
        opacity: 0.1,
        state: 'primary'
    });

    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');
    let url = `${urlParam}?` + query;

    let options = {
        method: 'POST',
        headers: {
            "X-ANTI-FORGERY-TOKEN": token,
        }
    };

    try {
        const response = await fetch(url, options);
        console.log(response)
        
        if (!response.ok) {
            var message;
            var responseJson = await response.json();
            switch (responseJson.MessageStatus) {
                case "NoAccessRight":
                    message = "Maaf, anda tidak memilik hak menggunakan modul ini...!";
                    throw new Error(message);
                    break;
                case "CookiesExpired":
                    window.location.href = baseUrl + '/Account/Login';
                    break;
                case "AntiForgeryTokenExpired":
                    window.location.href = baseUrl + '/Account/Login';
                    break;
                default:
                    message = 'Error : ' + responseJson.Message;
                    throw new Error(message);
                    break;
            }
        } else {
            return response;
        }
        return responseJson;
    } catch (error) {
        Swal.fire("Error", error.message, "error");
    } finally {
        KTApp.unblockPage();
    }
}

export { GetById, GetDataByParam, SubmitForm, DeleteById, DeleteByParam };
