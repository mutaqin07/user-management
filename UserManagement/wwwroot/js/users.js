"use strict";
import * as modul from './action-module.js';

let KTFormControls = function() {
	const uri = '/api/user/';
	const userClaimUri = '/api/userclaim/';
	const claimUri = '/api/claim/';

	function okResponse(title) {
		Swal.fire(
			"Information",
			title,
			"success"
		).then(function (result) {
			let oTable = $('#kt_datatable').DataTable();
			oTable.ajax.reload();
		})
	}

	let getClaimsByModule = function (userId, moduleId) {
		$('#claim_value').empty();
		$('#claim_value').append("<option value=''>Silahkan Pilih</option>");
		$('#claim_value').val('').trigger('change');

		if (userId != "" && moduleId > 0) {
			let params = {
				"moduleId": moduleId,
				"userId": userId
			};
			modul.GetDataByParam(claimUri + 'GetClaimsByModule', params)
				.then(data => {
					$('#claim_value').empty();
					$('#claim_value').append("<option value=''>Silahkan Pilih</option>");
					$.each(data, function (index, row) {
						$('#claim_value').append("<option value='" + row.claim_name + "'>" + row.claim_name + "</option>")
					});

					$('#claim_value').select2({
						placeholder: "Silahkan Pilih",
						allowClear: true
					});
					$('#claim_value').trigger('change');
				});
			//$.post(claimUri + 'GetClaimsByModule',
			//	{
			//		moduleId: moduleId,
			//		userId: userId
			//	},
			//	function (data) {
			//		$('#claim_value').empty();
			//		$('#claim_value').append("<option value=''>Silahkan Pilih</option>");
			//		$.each(data, function (index, row) {
			//			$('#claim_value').append("<option value='" + row.mc_id + "'>" + row.mc_name + "</option>")
			//		});

			//		$('#claim_value').select2({
			//			placeholder: "Silahkan Pilih",
			//			allowClear: true
			//		});
			//		$('#claim_value').trigger('change');
			//});
		} //else {
		//	$('#claim_value').empty();
		//	$('#claim_value').append("<option value=''>Silahkan Pilih</option>");
		//	$('#claim_value').val('').trigger('change');
		//}
	};

	let resetFormUserClaim = function () {
		$('#kt_form_2')[0].reset();
		$('#claim_value').empty();
		$('#claim_value').append("<option value=''>Silahkan Pilih</option>");
		$('#claim_value').val('').trigger('change');
	};

	let initForm = function () {
		const form = document.getElementById('kt_form_1');
		const fv = FormValidation.formValidation(form, {
				fields: {
					UserName: {
						validators: {
							notEmpty: {
								message: 'User Name is required'
							}
						}
					},
					FullName: {
						validators: {
							notEmpty: {
								message: 'Nama Lengkap is required'
							}
						}
					},
					Email: {
						validators: {
							notEmpty: {
								message: 'Email is required'
							},
							emailAddress: {
								message: 'The value is not a valid email address'
							}
						}
					},
					PhoneNumber: {
						validators: {
							notEmpty: {
								message: 'Phone number is required'
							},
							digits: {
								message: 'The value is not a valid phone number/digits only'
							}
						}
					},
					Password: {
						validators: {
							notEmpty: {
								message: 'Password is required'
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
					formData.append('Id', $('#hdnId').val());
					formData.append('UserName', $('#UserName').val());
					formData.append('FullName', $("#FullName").val());
					formData.append('Email', $('#Email').val());
					formData.append('PhoneNumber', $("#PhoneNumber").val());
					formData.append('Password', $('#Password').val());

					let id = $('#hdnId').val();
					let url = (id === '') ? uri + 'Add' : uri + 'Update';
					modul.SubmitForm(url, formData)
						.then(data => {
							okResponse(`User ${data.UserName} berhasil disimpan`);
							$('#kt_datatable_modal').modal('hide');
						});
				}
			});

		});

		var table = $('#kt_datatable');
		$.fn.dataTable.ext.errMode = 'none';

		// begin first table
		table.DataTable({
			responsive: true,
			searchDelay: 500,
			processing: true,
			serverSide: true,
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
				{ data: 'UserName' },
				{ data: 'FullName' },
				{
					"data": "actions", orderable: false, render: function (data, type, row) {
						return '\
						<div class="d-flex align-items-center">\
                            <div class="ml-3">\
                                <span class="text-dark-75 font-weight-bold line-height-sm d-block pb-2">'+ row.FullName + '</span>\
                                <a href="#" class="text-muted text-hover-primary">'+ row.Id + ' - ' + row.UserName + ' </a>\
                            </div>\
                        </div >\
						';
					}
				},
				{ data: 'Email' },
				{ data: 'IsActive' },
				{
					"data": "actions", orderable: false, render: function (data, type, row) {
						return '<a href="javascript:;" data-detail-id="' + row.Id + '" data-detail-name="' + row.FullName + '" class="btn btn-sm btn-info"> Claims </a> '
					}
				},
				{ data: 'Id', responsivePriority: -1 },
			],
			columnDefs: [
				{
					targets: -1,
					title: 'Actions',
					orderable: false,
					render: function (data, type, row) {
						return '\
							<a href="javascript:;" data-edit-id="' + row.Id + '" class="btn btn-sm btn-clean btn-icon" title="Edit details">\
								<i class="la la-edit"></i>\
							</a>\
							<a href="javascript:;" data-delete-id="' + row.Id + '"  class="btn btn-sm btn-clean btn-icon" title="Delete">\
								<i class="la la-exchange"></i>\
							</a>\
						';
					},
				},
				{ "className": "dt-center", "targets": [4, 5, 6] },
				{ "className": "hide_me", "targets": [0, 1] },
			],
		});

		table.on('click', '[data-edit-id]', function () {
			let id = $(this).data('edit-id');
			if (id != null && id != "") {
				$('#kt_datatable_modal').modal('show');
				$('#hdnId').val(id);
				$('.input-password').hide();
				$('#UserName').prop('disabled', true);
				modul.GetById(uri + 'GetById', id)
					.then(data => {
						$('#UserName').val(data.UserName);
						$('#FullName').val(data.FullName);
						$('#Email').val(data.Email);
						$('#PhoneNumber').val(data.PhoneNumber);
						$('#Password').val(data.PasswordHash);
					});
			}
		});

		table.on('click', '[data-delete-id]', function () {
			let id = $(this).data('delete-id');
			if (id != null && id != "") {
				Swal.fire({
					title: "Apakah anda yakin untuk merubah status User?",
					text: "Ganti Status",
					icon: "warning",
					showCancelButton: true,
					confirmButtonText: "Ya, rubah!",
					cancelButtonText: "Tidak, batal!",
					reverseButtons: true
				}).then(function (result) {
					if (result.value) {
						let formData = new FormData();
						formData.append('id', id);
						modul.SubmitForm(uri + 'ChangeStatus', formData)
							.then(data => {
								okResponse(`Status berhasil dirubah.`);
							});
					}
				});
			}
		});

		table.on('click', '[data-detail-id]', function () {
			let id = $(this).data('detail-id');
			let fullName = $(this).data('detail-name');
			if (id != null && id != '') {
				resetFormUserClaim();
				$('#claim_user_id').val(id);
				$('#userClaim_modal').modal('show');
				$('#userClaim-title').text(fullName);
				userClaimInitTable(id);

			}
		});

		$('#new-record').on('click', function () {
			$('#kt_form_1')[0].reset();
			$('#kt_datatable_modal').modal('show');
			$('#hdnId').val('');
			$('.input-password').show();
			$('#UserName').prop('disabled', false);
		});

		$(document).on('change', '#module_id', function () {
			let userId = $('#claim_user_id').val();
			let moduleId = $('#module_id').val();
			
			getClaimsByModule(userId, moduleId);
		});

		$('#claim_value').select2({
			placeholder: "Silahkan Pilih",
			allowClear: true,
			width: '100%'
		});
	};

	let userClaimInitTable = function (id) {
		let userClaimTable = $('#userClaim_kt_datatable');
		$.fn.dataTable.ext.errMode = 'none';
		// begin first table
		userClaimTable.DataTable({
			responsive: true,
			searchDelay: 500,
			processing: true,
			serverSide: true,
			destroy: true,
			ajax: {
				url: userClaimUri + 'GetDatatables',
				type: "POST",
				"data": function (d) {
					d.userId = id
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
				{ data: 'ClaimType' },
				{ data: 'ClaimValue' },
				{ data: 'Id', responsivePriority: -1 },
			],
			columnDefs: [
				{
					targets: -1,
					title: 'Hapus',
					orderable: false,
					render: function (data, type, row) {
						return '\
							<a href="javascript:;" data-delete-id="' + row.Id + '"  class="btn btn-sm btn-clean btn-icon" title="Delete">\
								<i class="la la-trash"></i>\
							</a>\
						';
					},
				},
				{ "className": "dt-center", "targets": [2] },
			],
		});


		userClaimTable.on('click', '[data-delete-id]', function () {
			let id = $(this).data('delete-id');
			if (id != null && id != '') {
				
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
						
						modul.DeleteById(userClaimUri + 'Delete', id)
							.then(data => {
								let dTable = $('#userClaim_kt_datatable').DataTable();
								dTable.ajax.reload();
							});
					}
				});
			}
		});
	};

	var initValidation = function () {
		FormValidation.formValidation(
			document.getElementById('kt_form_2'),
			{
				fields: {
					module_id: {
						validators: {
							notEmpty: {
								message: 'Nama module is required'
							}
						}
					},
					claim_value: {
						validators: {
							notEmpty: {
								message: 'Claims is required'
							}
						}
					}
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
					formData.append('UserId', $('#claim_user_id').val());
					formData.append('ClaimType', $("#claim_value").val());
					formData.append('ClaimValue', $("#claim_value").val());

					modul.SubmitForm(userClaimUri + 'Add', formData)
						.then(data => {
							let dTable = $('#userClaim_kt_datatable').DataTable();
							dTable.ajax.reload();
							let userId = $('#claim_user_id').val();
							let moduleId = $('#module_id').val();

							getClaimsByModule(userId, moduleId);
						});
				}
			});

		});
	};

	return {

		//main function to initiate the module
		init: function () {
			initValidation();
			initForm();
		},

	};

}();

jQuery(document).ready(function() {
	KTFormControls.init();
});
