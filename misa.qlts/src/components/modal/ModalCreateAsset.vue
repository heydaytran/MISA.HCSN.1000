<template>
  <div>
    <div v-if="isActive" class="modal">
      <div class="modal-background"></div>
      <div class="modal-content">
        <div class="header">
          <div v-if="formMode == 'insert'" class="title">Thêm mới tài sản</div>
          <div v-else class="title">Sửa tài sản</div>

          <div class="header-right">
            <div class="icon-help btn btn-help"></div>
            <div class="icon-cancel btn btn-close" @click="hide()"></div>
          </div>
        </div>

        <div class="content">
          <div class="input-field">
            <label for="">Mã tài sản (<span>*</span>)</label>
            <input
              id="assetInput1"
              type="text"
              maxlength="20"
              class="input-one-third"
              v-model="asset.assetCode"
              @keyup="validateAssetCode()"
              @blur="validateAssetCode()"
            />

            <div id="assetInput1_warning" class="validate-warning">
              Thông tin bắt buộc
            </div>
          </div>
          <div class="input-field">
            <label for="">Tên tài sản (<span>*</span>)</label>
            <input
              id="assetInput2"
              maxlength="255"
              class="input-two-third"
              type="text"
              @keyup="validateAssetName()"
              @blur="validateAssetName()"
              v-model="asset.assetName"
            />

            <div id="assetInput2_warning" class="validate-warning">
              Thông tin bắt buộc
            </div>
          </div>
          <div class="clear-float"></div>
          <div class="input-field">
            <label>Mã phòng ban</label>
            <v-autocomplete
              id="assetInput3"
              class="custom-autocomplete input-one-third"
              :items="listDepartment"
              item-value="departmentId"
              item-text="departmentCode"
              v-model="asset.departmentId"
            ></v-autocomplete>
          </div>
          <div class="input-field">
            <label for="">Tên phòng ban</label>
            <input
              class="input-black input-two-third"
              tabindex="-1"
              type="text"
              name=""
              v-model="asset.departmentName"
            />
          </div>
          <div class="clear-float"></div>
          <div class="input-field">
            <label for="">Mã loại tài sản</label>
            <v-autocomplete
              id="assetInput4"
              class="custom-autocomplete input-one-third"
              :items="listAssetType"
              item-value="assetTypeId"
              item-text="assetTypeCode"
              v-model="asset.assetTypeId"
            ></v-autocomplete>
          </div>
          <div class="input-field">
            <label for="">Tên loại tài sản</label>
            <input
              class="input-black input-two-third"
              tabindex="-1"
              type="text"
              name=""
              v-model="asset.assetTypeName"
            />
          </div>
          <div class="clear-float"></div>
          <div class="input-field">
            <label for="">Thời gian sử dụng (năm)</label>
            <input
              id="assetInput5"
              class="input-one-third"
              type="text"
              name=""
              @keypress="formatNumber($event)"
              v-model="asset.timeUse"
            />
          </div>
          <div class="input-field">
            <label for="">Tỷ lệ hao mòn (%)</label>
            <input
              id="assetInput6"
              class="input-one-third"
              maxlength="10"
              type="text"
              name=""
              @keypress="formatNumber($event)"
              v-model="asset.wearRate"
            />
          </div>
          <div class="input-field">
            <label for="">Nguyên giá</label>
            <input
              id="assetInput7"
              maxlength="13"
              class="input-one-third"
              type="text"
              name=""
              style="text-align: right"
              :value="formatedMoney"
              @keypress="formatNumber($event)"
              @keyup="updateInput($event)"
            />
          </div>
          <div class="clear-float"></div>
          <div class="input-field">
            <label for="">Giá trị hao mòn năm</label>
            <input
              style="text-align: right"
              id="assetInput8"
              maxlength="10"
              class="input-one-third"
              type="text"
              name=""
              @keypress="formatNumber($event)"
              v-model="asset.wearValue"
            />
          </div>
        </div>
        <div class="footer">
          <div class="btn btn-cancel" tabindex="0" @click="hide()">Hủy</div>

          <div class="btn btn-save" tabindex="0" @click="save()">Lưu</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
  props: {
    listAssetType: Array,
    listDepartment: Array,
    formMode: String,
    assetIdUpdate: String,
  },
  data() {
    return {
      isActive: false,
      isSuccess: true,
      asset: {
        assetId: null,
        assetCode: null,
        assetName: null,
        assetTypeId: null,
        departmentId: null,
        timeUse: null,
        wearRate: null,
        originalPrice: null,
        wearValue: null,
        isUsed: false,
        departmentName: null,
        assetTypeName: null,
        createdBy: null,
        modifiedBy: null,
        createDate: null,
      },
    };
  },
  methods: {
    resetInput() {
      (this.asset.assetId = null),
        (this.asset.assetCode = null),
        (this.asset.assetName = null),
        (this.asset.assetTypeId = null),
        (this.asset.departmentId = null),
        (this.asset.timeUse = null),
        (this.asset.wearRate = null),
        (this.asset.originalPrice = null),
        (this.asset.wearValue = null),
        (this.asset.isUsed = false),
        (this.asset.departmentName = null),
        (this.asset.assetTypeName = null),
        (this.asset.createdBy = null),
        (this.asset.modifiedBy = null);
    },
    // todo hiện dialog
    async show() {
      var res = this;
      this.isActive = true;

      // focus vào input đầu tiên( mã tài sản)
      setTimeout(() => {
        document.getElementById("assetInput1").focus();
      }, 0);
      document.getElementsByClassName("body-right")[0].style.zIndex = "999";
      if (this.formMode == "update") {
        await axios
          .get("https://localhost:44382/api/v1/Assets/" + this.assetIdUpdate)
          .then((Response) => {
            res.asset.departmentId = "";
            res.asset = Response.data.data;

            // debugger; // eslint-disable-line no-debugger
          })
          .catch((error) => {
            this.errorMessage = error.message;
            console.error("GET Asset by id Failed: ", error.message);
          });

          // convert ngyên giá sang string
        var a = res.asset.originalPrice;
        if (a == null) a = "";
        else a = res.asset.originalPrice.toString();

        res.asset.originalPrice = a;
        //debugger; // eslint-disable-line no-debugger
      } else {
        this.resetInput();
      }
    },

    // todo ẩn dialog
    hide() {
      this.isActive = false;
      document.getElementsByClassName("body-right")[0].style.zIndex = "0";
    },

    // todo lấy dữ liệu tên phòng ban
    getDepartmentName() {
      var res = this;
      console.log(this.listAssetType, "list deparment");
      this.listDepartment.forEach((element) => {
        console.log(element.departmentId, "element.departmentId");
        console.log(res.asset.departmentId, "res.asset.departmentId");
        if (element.departmentId == res.asset.departmentId) {
          res.asset.departmentName = element.departmentName;
        }
      });
    },

    //todo lấy dữ liệu tên loại tài sản
    getAssetTypeName() {
      var res = this;
      this.listAssetType.forEach((element) => {
        if (element.assetTypeId == res.asset.assetTypeId) {
          res.asset.assetTypeName = element.assetTypeName;
        }
      });
    },

    // todo chỉ cho phép nhập số
    formatNumber(e) {
      var key = e.key;
      if (!/^\d+/g.test(key)) {
        e.preventDefault();
      }
    },

    //todo định dạng kiểu tiền tệ
    formatMoney(money) {
      return money.replace(/\B(?=(\d{3})+(?!\d))/g, `,`);
    },

    // todo bỏ định dạng tiền tệ
    removeFormatMoney(money) {
      return money.replace(/\D+/g, "");
    },

    // update input
    updateInput(e) {
      this.asset.originalPrice = this.removeFormatMoney(e.target.value);
    },

    // validate trống trường dữ liệu mã tài sản
    async validateAssetCode() {
      var warning = document.getElementById("assetInput1");
      if (this.asset.assetCode == null || this.asset.assetCode == "") {
        warning.style.border = "1px solid red";
        warning.classList.add("hover-validate");
      } else {
        warning.style.border = "#e4e4e4 1px solid";
        warning.classList.remove("hover-validate");
      }
    },

    // todo validate trường dữ liệu tên tài sản
    validateAssetName() {
      var warning = document.getElementById("assetInput2");
      if (this.asset.assetName == null || this.asset.assetName == "") {
        warning.style.border = "1px solid red";
        warning.classList.add("hover-validate");
      } else {
        warning.style.border = "#e4e4e4 1px solid";
        warning.classList.remove("hover-validate");
      }
    },

    // todo lưu dữ liệu
    async save() {
      this.validateAssetName();
      this.validateAssetCode();
      var res = this;

      if (this.asset.assetCode == null || this.asset.assetName == null) {
        return;
      } else {
        if (this.formMode == "insert") {
          //nếu là form thêm dữ liệu
          res.asset.createDate = new Date(Date.now());
          console.log("đã vô else cbi gọi axios");
          console.log(this.asset, "dữ liệu cbi được thêm");
          await axios
            .post("https://localhost:44382/api/v1/assets/", this.asset)
            .then((respone) => {
              document.getElementById("assetInput1_warning").innerText =
                respone.data.userMsg;
              document.getElementById("assetInput1").style.border =
                "1px solid red";
              document
                .getElementById("assetInput1")
                .classList.add("hover-validate");
              if (respone.data.errorCode != 400) {
                res.hide();
                res.$emit("reload", true);
              } else {
                res.$emit("reload", false);
                return;
              }
            })
            .catch((error) => {
              res.$emit("reload", false);
              console.log(error);
              alert("Có lỗi xảy ra, vui lòng liên hệ MISA để được trợ giúp")
            });
        } else {
          // nếu là form sửa dữ liệu
          await axios
            .put("https://localhost:44382/api/v1/assets/", this.asset)
            .then((respone) => {
              if (respone.data.errorCode != 400) {
                res.hide();
                res.$emit("reload", true);
              } else {
                res.$emit("reload", false);
              }
            })
            .catch((error) => {
              console.log(error);
              res.$emit("reload", false);
              alert("Có lỗi xảy ra, vui lòng liên hệ MISA để được trợ giúp!");
            });
        }
      }
    },
  },
  watch: {
    "asset.departmentId": function () {
      this.getDepartmentName();
    },
    "asset.assetTypeId": function () {
      this.getAssetTypeName();
    },
  },
  computed: {
    formatedMoney() {
      var a = this.asset.originalPrice;
      if (a == null || a == "") a = "";
      else a = a.toString();
      return this.formatMoney(a);
    },
  },
  filters: {},
};
</script>


<style lang='scss' scoped>
.test-autocomplete {
  border: 1px solid black;
}

.modal .validate-warning {
  font-family: GoogleSans-Regular;
  font-size: 13px;
  color: red;
  background-color: white;
  /* z-index: 22; */
  position: absolute;
  bottom: 8px;
  right: 14px;
}

.modal-content {
  position: absolute;
  top: calc(50% - 275px);
  left: calc(50% - 350px);
  width: 705px;
  height: 530px;
  background-color: white;
  resize: both;
  overflow: auto;
  min-height: 530px;
  min-width: 705px;
  height: 530px;
  width: 705px;

  .header {
    width: 100%;
    height: 60px;
    display: flex;
    padding: 0 16px;
    box-sizing: border-box;
    display: flex;
    align-items: center;

    .title {
      line-height: 50px;
      font-family: GoogleSans-Bold;
      font-size: 20px;
    }

    .header-right {
      position: absolute;
      right: 0;
      display: flex;
      align-items: center;
      padding: 0 12px;

      .icon-cancel {
        background-size: 16px;
        margin-left: 2px;
      }
    }
  }

  .content {
    width: 100%;
    height: calc(100% - 110px);
    padding: 16px 0px 16px 16px;
    box-sizing: border-box;
    overflow-y: auto;
    overflow-x: hidden;

    .input-field {
      float: left;
      padding: 0 16px 16px 0px;
      position: relative;

      label {
        display: block;
        padding: 2px 2px 4px 2px;
      }

      input {
        height: 34px;
        box-sizing: border-box;
        border: #e4e4e4 1px solid;
        // outline-color: lightgreen;
        padding: 10px;
      }
    }
  }

  .footer {
    width: 100%;
    height: 50px;
    background-color: #f5f5f5;
    display: flex;
    align-items: center;
    position: relative;
    padding: 0 16px;
  }
}

.validate-warning {
  display: none;
}
.hover-validate:hover ~ .validate-warning {
  display: block;
}
.modal-content .header .title {
  font-family: GoogleSans-Bold !important;
}
</style>