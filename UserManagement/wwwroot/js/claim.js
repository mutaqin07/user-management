"use strict";
import * as modul from './action-module.js';

var KTFormControls = function () {
    const uri = '/api/claim/';

    function OkResponse(title) {
        Swal.fire(
            "Information",
            title,
            "success"
        ).then(function (result) {
            let oTable = $('#kt_datatable').DataTable();
            oTable.ajax.reload();
        })

    }
    var initTable = function () {
        var table = $('#kt_datatable');
        $.fn.dataTable.ext.errMode = 'none';
        // begin first table
        table.DataTable({
            responsive: true,
            searchDelay: 500,
            processing: true,
            serverSide: true,
            destroy: true,
            ajax: {
                url: uri + 'GetDatatables',
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
                { data: 'module_id' },
                { data: 'description' },
                { data: 'claim_name', responsivePriority: -1 },
            ],
            columnDefs: [
                {
                    targets: -1,
                    title: 'Actions',
                    orderable: false,
                    render: function (data, type, row) {
                        return '\
							<a href="javascript:;" data-edit-id="' + row.claim_name + '" class="btn btn-sm btn-clean btn-icon" title="Edit details">\
								<i class="la la-edit"></i>\
							</a>\
							<a href="javascript:;" data-delete-id="' + row.claim_name + '"  class="btn btn-sm btn-clean btn-icon" title="Delete">\
								<i class="la la-trash"></i>\
							</a>\
						';
                    },
                },
                { "className": "dt-center", "targets": [1, 3] },
            ],
        });

        table.on('click', '[data-edit-id]', function () {
            let id = $(this).data('edit-id');
            if (id != null && id != "") {
                $('#kt_form_1')[0].reset();
                $('#claim_name').prop('readonly', true);
                $('#act').val('Update');
                $('#claim_name').val(id);
                $('#kt_datatable_modal').modal('show');
                modul.GetById(uri + 'GetById', id)
                    .then(data => {
                        $('#module_id').val(data.module_id);
                        $('#description').val(data.description);
                    });
            }
        });

        table.on('click', '[data-delete-id]', function () {
            let id = $(this).data('delete-id');
            if (id != null && id != "") {
                Swal.fire({
                    title: "Apakah anda yakin untuk menghapus "+ id +"?",
                    text: "Anda tidak bisa membatalkan ini!",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Ya, hapus!",
                    cancelButtonText: "Tidak, batal!",
                    reverseButtons: true
                }).then(function (result) {
                    if (result.value) {
                        modul.DeleteById(uri + 'Delete', id)
                            .then(data => {
                                OkResponse('Module berhasil dihapus');
                            });
                    }
                });
            }
        });
    };

    var initValidation = function () {
        FormValidation.formValidation(
            document.getElementById('kt_form_1'),
            {
                fields: {
                    claim_name: {
                        validators: {
                            notEmpty: {
                                message: 'Nama claim is required'
                            }
                        }
                    },
                    module_id: {
                        validators: {
                            notEmpty: {
                                message: 'Nama module is required'
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
                    icon: new FormValidation.plugins.Icon({
                        valid: 'fa fa-check',
                        invalid: 'fa fa-times',
                        validating: 'fa fa-refresh',
                    }),
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
                    //console.log($('#module_id option:selected').text());
                    let formData = new FormData();
                    formData.append('claim_name', $("#claim_name").val());
                    formData.append('module_id', $('#module_id').val());
                    formData.append('description', $("#description").val());
                    formData.append('moduleName', $('#module_id option:selected').text());

                    let act = $('#act').val();
                    modul.SubmitForm(uri + act, formData)
                        .then(data => {
                            OkResponse(`Claims ${data.claim_name} berhasil disimpan`);
                            $('#kt_datatable_modal').modal('hide');
                        });
                }
            });

        });
    };

    var initAction = function () {
        $('#new-record').on('click', function () {
            $('#claim_name').prop('readonly', false);
            $('#kt_form_1')[0].reset();
            $('#act').val('Add');
            $('#kt_datatable_modal').modal('show');
        });

    };

    return {

        //main function to initiate the module
        init: function () {
            initTable();
            initValidation();
            initAction();
        },

    };

}();

jQuery(document).ready(function () {
    KTFormControls.init();
});
