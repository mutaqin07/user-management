"use strict";
import * as modul from './action-module.js';

var KTFormControls = function () {
    const uri = '/api/policy/';
    const detailUri = '/api/detailpolicy/';

    function OkResponse(title) {
        Swal.fire(
            "Information",
            title,
            "success"
        ).then(function (result) {
            let oTable = $('#kt_datatable').DataTable();
            oTable.ajax.reload();
        })
    };

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
                { data: 'policy_id' },
                { data: 'policy_name' },
                { data: 'description' },
                {
                    "data": "actions", orderable: false, render: function (data, type, row) {
                        return '<a href="javascript:;" data-detail-id="' + row.policy_id + '" data-detail-name="' + row.policy_name + '" class="btn btn-sm btn-info"> Claims </a> '
                    }
                },
                { data: 'policy_id', responsivePriority: -1 },
            ],
            columnDefs: [
                {
                    targets: -1,
                    title: 'Actions',
                    orderable: false,
                    render: function (data, type, row) {
                        return '\
							<a href="javascript:;" data-edit-id="' + row.policy_id + '" class="btn btn-sm btn-clean btn-icon" title="Edit details">\
								<i class="la la-edit"></i>\
							</a>\
							<a href="javascript:;" data-delete-id="' + row.policy_id + '"  class="btn btn-sm btn-clean btn-icon" title="Delete">\
								<i class="la la-trash"></i>\
							</a>\
						';
                    },
                },
                { "className": "dt-center", "targets": [0, 3, 4] },
            ],
        });

        table.on('click', '[data-edit-id]', function () {
            let id = $(this).data('edit-id');
            if (id != null && id > 0) {
                $('#policy_id').val(id);
                $('#kt_datatable_modal').modal('show');
                modul.GetById(uri + 'GetById', id)
                    .then(data => {
                        $('#policy_name').val(data.policy_name);
                        $('#description').val(data.description);
                    });
            }
        });

        table.on('click', '[data-delete-id]', function () {
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
                        modul.DeleteById(uri + 'Delete', id)
                            .then(data => {
                                OkResponse('Module berhasil dihapus');
                            });
                    }
                });
            }
        });

        table.on('click', '[data-detail-id]', function () {
            let id = $(this).data('detail-id');
            let policyName = $(this).data('detail-name');
            if (id != null && id > 0) {
                $('#kt_form_1')[0].reset();
                $('#claim_policy_id').val(id);
                $('#claim_modal').modal('show');
                $('#dtl_policy_id').val(id);
                $('#dtl_policy_name').val('Policy: ' + policyName);
                detailInitTable(id);
            }
        });
    };

    var initValidation = function () {
        FormValidation.formValidation(
            document.getElementById('kt_form_1'),
            {
                fields: {
                    policy_name: {
                        validators: {
                            notEmpty: {
                                message: 'Nama policy is required'
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
                    let formData = new FormData();
                    formData.append('policy_id', $('#policy_id').val());
                    formData.append('policy_name', $("#policy_name").val());
                    formData.append('description', $('#description').val());

                    let policyId = $('#policy_id').val();
                    let url = '';
                    if (policyId > 0) {
                        url = uri + 'Update';
                    } else {
                        url = uri + 'Add';
                    }
                    modul.SubmitForm(url, formData)
                        .then(data => {
                            OkResponse(`Policy ${data.policy_name} berhasil disimpan`);
                            $('#kt_datatable_modal').modal('hide');
                        });
                }
            });

        });
    };

    var initAction = function () {
        $('#new-record').on('click', function () {
            $('#kt_form_1')[0].reset();
            $('#policy_id').val(0);
            $('#kt_datatable_modal').modal('show');
        });
    };


    var detailInitTable = function (id) {
        var detailTable = $('#detail_kt_datatable');
        $.fn.dataTable.ext.errMode = 'none';
        // begin first table
        detailTable.DataTable({
            responsive: true,
            searchDelay: 500,
            processing: true,
            serverSide: true,
            destroy: true,
            ajax: {
                url: detailUri + 'GetDatatables',
                type: "POST",
                "data": function (d) {
                    d.id = id
                },
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
                { data: 'policy_id', responsivePriority: -1 },
            ],
            columnDefs: [
                {
                    targets: -1,
                    title: 'Hapus',
                    orderable: false,
                    render: function (data, type, row) {
                        return '\
							<a href="javascript:;" data-delete-id="' + row.policy_id + '|' + row.claim_name +'"  class="btn btn-sm btn-clean btn-icon" title="Delete">\
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
            if (id != null && id != '') {
                let idSpllit = id.split('|');
                let policyId = idSpllit[0];
                let claimName = idSpllit[1];
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
                        let params = {
                            "policyId": policyId,
                            "claimName": claimName
                        };
                        modul.DeleteByParam(detailUri + 'Delete', params)
                            .then(data => {
                                let dTable = $('#detail_kt_datatable').DataTable();
                                dTable.ajax.reload();
                            });
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

    };

    return {

        //main function to initiate the module
        init: function () {
            initTable();
            initValidation();
            initAction();

            detailInitValidation();
            detailInitAction();
        },

    };

}();

jQuery(document).ready(function () {
    KTFormControls.init();
});
