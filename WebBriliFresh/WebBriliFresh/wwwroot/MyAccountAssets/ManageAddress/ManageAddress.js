/* Load modal sửa địa chỉ khi nhấn vào nút 'Cập nhật' */
const editBtns = document.querySelectorAll('.js-edit-address')
const modalEdit = document.querySelector('.js-modal-edit')
const modalEditClose = document.querySelector('.js-modalEdit-close')
var modalContainer = document.querySelector('.js-modal-container')

function showEditAddress(addressId) {
    if (addressId != 'Update') {
        modalContainer.setAttribute("action", "/MyAccount/UpdateAddress/" + addressId)
    } else {
        modalContainer.setAttribute("action", "/MyAccount/AddAddress")
    }
    modalEdit.classList.add('open')
    var input = document.createElement('input')
    input.value = addressId
    input.name = "AddId"
    input.type = "number"
    input.setAttribute("hidden", true)
    modalContainer.appendChild(input)
}

function hideEditAddress() {
    modalEdit.classList.remove('open')
}


modalEditClose.addEventListener('click', hideEditAddress);


modalContainer.addEventListener('click', function (event) {
    event.stopPropagation()     // ngăn chặn việc nổi bọt
});



/* Load modal thêm mới địa chỉ khi nhấn vào nút 'Thêm địa chỉ' */
const addBtn = document.querySelector('#js-newaddress-btn')

function showAddAddress() {
    modalEdit.classList.add('open')
}

function hideAddAddress() {
    modalEdit.classList.remove('open')
}

document.getElementsByClassName('modal-header')[0].innerHTML = 'Thêm địa chỉ mới'

addBtn.addEventListener('click', showAddAddress)
modalEdit.addEventListener('click', hideAddAddress);



/* Load modal xác nhận xóa địa chỉ khi nhấn vào nút 'Xóa' */
const deleteBtns = document.querySelectorAll('.js-delete-address')
const modalDelete = document.querySelector('.js-modal-delete')
const modalDeleteClose = document.querySelector('.js-modalDelete-close')

function showDeleteAddress() {
    modalDelete.classList.add('open')
}

function hideDeleteAddress() {
    modalDelete.classList.remove('open')
}

for (const deleteBtn of deleteBtns) {
    deleteBtn.addEventListener('click', showDeleteAddress)
}
modalDeleteClose.addEventListener('click', hideDeleteAddress);



/* Lấy & load dữ liệu vào dropdown địa chỉ Việt Nam từ API */

$(document).ready(function () {
    //Combobox địa chỉ
    var citis = document.getElementById("city");
    var district = document.getElementById("district");
    var wards = document.getElementById("ward");
    var Parameter = {
        url: "https://raw.githubusercontent.com/kenzouno1/DiaGioiHanhChinhVN/master/data.json",
        method: "GET",
        responseType: "application/json",
    };
    var promise = axios(Parameter);
    promise.then(function (result) {
        renderCity(result.data);
    });

    function renderCity(data) {
        for (const x of data) {
            citis.options[citis.options.length] = new Option(x.Name, x.Name);
        }
        citis.onchange = function () {
            district.length = 1;
            ward.length = 1;
            if (this.value != "") {
                const result = data.filter(n => n.Name === this.value);

                for (const k of result[0].Districts) {
                    district.options[district.options.length] = new Option(k.Name, k.Name);
                }
            }
        };
        district.onchange = function () {
            ward.length = 1;
            const dataCity = data.filter((n) => n.Name === citis.value);
            if (this.value != "") {
                const dataWards = dataCity[0].Districts.filter(n => n.Name === this.value)[0].Wards;

                for (const w of dataWards) {
                    wards.options[wards.options.length] = new Option(w.Name, w.Name);
                }
            }
        };
    }
});