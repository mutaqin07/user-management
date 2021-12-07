
function OkResponse() {
    Swal.fire(
        "Submited",
        "Mantapppp",
        "success"
    )
}

document.addEventListener('DOMContentLoaded', function () {

    $(document).on('submit', '#TestForm1', function (event) {
        event.preventDefault();

        //var token = $('input[name="__RequestVerificationToken"]', TestForm1).val();
        var formData = new FormData();
        formData.append('NIK', "A02");
        formData.append('Nama', "Hafiz");
        //formData.append('__RequestVerificationToken', token);

        SubmitForm("Apakah anda yakin...?", "Menghapus data ini", "Ya, Hapus!", "Batal", 'MainMenu/SubmitForm1', formData, OkResponse);
    });

    $(document).on('submit', '#TestForm2', function (event) {
        event.preventDefault();

        var formData = new FormData();
        formData.append('NIK', "A02");
        formData.append('Nama', "Hafiz");

        SubmitForm("Apakah anda yakin...?", "Menghapus data ini", "Ya, Hapus!", "Batal", 'MainMenu/SubmitForm2', formData, OkResponse);
    });

    $(document).on('submit', '#TestForm3', function (event) {
        event.preventDefault();
       
        var formData = new FormData();
        formData.append('NIK', "A02");
        formData.append('Nama', "Hafiz");
       
        SubmitForm("Apakah anda yakin...?", "Menghapus data ini", "Ya, Hapus!", "Batal", 'MainMenu/SubmitForm3', formData, OkResponse);
    });
}, false);