"use strict";
import * as modul from './action-module.js';

var DetailKTFormControls = function () {
    const detailUri = '/api/detailpolicy/';

    //function OkResponse(title) {
    //    Swal.fire(
    //        "Information",
    //        title,
    //        "success"
    //    ).then(function (result) {
    //        let oTable = $('#kt_datatable').DataTable();
    //        oTable.ajax.reload();
    //    })
    //}

    var detailInitTable = function () {
        var detailTable = $('#detail_kt_datatable');
        $.fn.dataTable.ext.errMode = 'none';
        // begin first table
        detailTable.DataTable({
            responsive: true,
            searchDelay: 500,
            processing: true,
            serverSide: true,
            ajax: {
                url: detailUri + 'GetDatatables',
                type: "POST",
                error: function (error) {
                    if (error.status == 401) {
                        window.location.href = "Account/Login";
                    }
                    else {
                        Swal.fire(
                            "Error : " + error.status,
                            error.responseJSON.message,
                            "error"
                        )
                    }
                }
            },
            columns: [
                { data: 'claim_name' },
                { data: 'id', responsivePriority: -1 },
            ],
            columnDefs: [
                {
                    targets: -1,
                    title: 'Hapus',
                    orderable: false,
                    render: function (data, type, row) {
                        return '\
							<a href="javascript:;" data-delete-id="' + row.id + '"  class="btn btn-sm btn-clean btn-icon" title="Delete">\
								<i class="la la-trash"></i>\
							</a>\
						';
                    },
                },
                { "className": "dt-center", "targets": [1] },
            ],
        });

       
        detailTable.on('click', '[data-delete-id]', function () {
            let id = $(this).data('delete-id');
            if (id != null && id > 0) {
                Swal.fire({
                    title: "Apakah anda yakin untuk menghapus?",
                    text: "Anda tidak bisa membatalkan ini!",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Ya, hapus!",
                    cancelButtonText: "Tidak, batal!",
                    reverseButtons: true
                }).then(function (result) {
                    if (result.value) {
                        Swal.fire(
                            "Deleted!",
                            "Your file has been deleted.",
                            "success"
                        )
                    }
                });
            }
        });
    };

    var detailInitValidation = function () {
        FormValidation.formValidation(
            document.getElementById('detail_kt_form_1'),
            {
                fields: {
                    dtl_policy_id: {
                        validators: {
                            notEmpty: {
                                message: 'Policy is required'
                            }
                        }
                    },
                    dtl_claim_name: {
                        validators: {
                            notEmpty: {
                                message: 'claim is required'
                            }
                        }
                    },
                },

                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    // Bootstrap Framework Integration
                    bootstrap: new FormValidation.plugins.Bootstrap(),
                    // Validate fields when clicking the Submit button
                    submitButton: new FormValidation.plugins.SubmitButton(),
                    // Submit the form when all fields are valid
                    //defaultSubmit: new FormValidation.plugins.DefaultSubmit(),
                }
            }
        ).on('core.form.valid', function () {
            Swal.fire({
                title: "Konfirmasi",
                text: "Apakah anda ingin menyimpan data ini?",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Ya, simpan!",
                cancelButtonText: "Tidak, batal!",
                reverseButtons: true
            }).then(function (result) {
                if (result.value) {
                    let formData = new FormData();
                    formData.append('policy_id', $('#dtl_policy_id').val());
                    formData.append('claim_name', $("#dtl_claim_name").val());

                    modul.SubmitForm(detailUri + 'Add', formData)
                        .then(data => {
                            let dTable = $('#detail_kt_datatable').DataTable();
                            dTable.ajax.reload();
                        });
                }
            });

        });
    };

    var detailInitAction = function () {
        $('#dtl_claim_name').select2({
            placeholder: "Silahkan Pilih",
            allowClear: true
        });

        //$('#new-record').on('click', function () {
        //    $('#kt_datatable_modal').modal('show');
        //});
    };

    return {

        //main function to initiate the module
        init: function () {
            //detailInitTable();
            detailInitValidation();
            detailInitAction();
        },

    };

}();

jQuery(document).ready(function () {
    DetailKTFormControls.init();
});
