<template>
  <div>
    <div v-if="isActive" class="modal">
      <div class="modal-background"></div>
      <div class="modal-content">
        <div class="header">
          <div v-if="formMode=='insert'" class="title">Thêm mới tài sản</div>
          <div v-else class="title">Sửa tài sản</div>

          <div class="header-right">
            <div class="icon-help btn btn-help"></div>
            <div class="icon-cancel btn btn-close" @click="hide()"></div>
          </div>
        </div>

        <div class="content">
          <div class="input-field">
            <label for="">Mã tài sản (<span>*</span>)</label>
            <input id="assetInput1" type="text" maxlength="20" class="input-one-third" v-model="asset.assetCode"
              @keyup="validateAssetCode()" 
              @blur="validateAssetCode()"
              />

            <div id="assetInput1_warning" class="validate-warning">
              Không được để trống
            </div>
          </div>
          <div class="input-field">
            <label for="">Tên tài sản (<span>*</span>)</label>
            <input id="assetInput2" maxlength="255"  class="input-two-third" type="text" 
            @keyup="validateAssetName()" 
              @blur="validateAssetName()"  
              v-model="asset.assetName"
            />

            <div id="assetInput2_warning"  class="validate-warning">
              Không được để trống
            </div>
          </div>
          <div class="clear-float"></div>
          <div class="input-field">
            <label>Mã phòng ban</label>
            <v-autocomplete id="assetInput3" class="custom-autocomplete input-one-third" :items="listDepartment"
              item-value="departmentId" item-text="departmentCode" v-model="asset.departmentId"></v-autocomplete>
          </div>
          <div class="input-field">
            <label for="">Tên phòng ban</label>
            <input class="input-black input-two-third" tabindex="-1" type="text" name=""
              v-model="asset.departmentName" />
          </div>
          <div class="clear-float"></div>
          <div class="input-field">
            <label for="">Mã loại tài sản</label>
            <v-autocomplete id="assetInput4" class="custom-autocomplete input-one-third" :items="listAssetType"
              item-value="assetTypeId" item-text="assetTypeCode" v-model="asset.assetTypeId"></v-autocomplete>
          </div>
          <div class="input-field">
            <label for="">Tên loại tài sản</label>
            <input class="input-black input-two-third" tabindex="-1" type="text" name=""
              v-model="asset.assetTypeName" />
          </div>
          <div class="clear-float"></div>
          <div class="input-field">
            <label for="">Thời gian sử dụng (năm)</label>
            <input id="assetInput5" class="input-one-third" type="text" name=""
              @keypress="handleKeyupOnlyNumber($event)" v-model="asset.timeUse" />
          </div>
          <div class="input-field">
            <label for="">Tỷ lệ hao mòn (%)</label>
            <input id="assetInput6" class="input-one-third" maxlength="10" type="text" name=""
              @keypress="handleKeyupOnlyNumber($event)" v-model="asset.wearRate" />
          </div>
          <div class="input-field">
            <label for="">Nguyên giá</label>
            <input id="assetInput7" maxlength="13" class="input-one-third" type="text" name="" style="text-align: right"
              v-model="asset.originalPrice" @keyup="formatMoney(asset.originalPrice)"
              @keypress="handleKeyupOnlyNumber($event)" />
          </div>
          <div class="clear-float"></div>
          <div class="input-field">
            <label for="">Giá trị hao mòn năm</label>
            <input style="text-align: right" id="assetInput8" maxlength="10" class="input-one-third" type="text" name=""
              @keypress="handleKeyupOnlyNumber($event)" v-model="asset.wearValue" />
          </div>
        </div>
        <div class="footer">
          <div class="btn btn-cancel" tabindex="0" @click="hide()">Hủy</div>

          <div class="btn btn-save" tabindex="0" @click="validateform()">Lưu</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
  import axios from 'axios';
  export default {
    props: {
      listAssetType: Array,
      listDepartment: Array,
      formMode: String,
      assetIdUpdate: String
    },
    data() {
      return {
        isActive: false,
        asset: {
          assetId: null,
          assetCode: null,
          assetName: null,
          assetTypeId: null,
          departmentId: null,
          increaseDate: "2021-03-06T00:00:00",
          timeUse: null,
          wearRate: null,
          originalPrice: null,
          wearValue: null,
          isUsed: false,
          departmentName: null,
          assetTypeName: null,
          createdDate: "0001-01-01T00:00:00",
          createdBy: null,
          modifiedDate: "0001-01-01T00:00:00",
          modifiedBy: null
        },
      };
    },
    methods: {
      resetInput() {
        this.asset.assetId = null,
          this.asset.assetCode = null,
          this.asset.assetName = null,
          this.asset.assetTypeId = null,
          this.asset.departmentId = null,
          this.asset.increaseDate = "2021-03-06T00=00=00",
          this.asset.timeUse = null,
          this.asset.wearRate = null,
          this.asset.originalPrice = null,
          this.asset.wearValue = null,
          this.asset.isUsed = false,
          this.asset.departmentName = null,
          this.asset.assetTypeName = null,
          this.asset.createdDate = "0001-01-01T00=00=00",
          this.asset.createdBy = null,
          this.asset.modifiedDate = "0001-01-01T00=00=00",
          this.asset.modifiedBy = null
      },
      async show() {
        var res = this
        this.isActive = true;
        setTimeout(() => {
          document.getElementById("assetInput1").focus();
        }, 0);
        document.getElementsByClassName("body-right")[0].style.zIndex = "999";
        if (this.formMode == 'update') {
          await axios.get('https://localhost:44382/api/v1/Assets/' + this.assetIdUpdate)
            .then((Response) => {
              res.asset.departmentId = ''
              res.asset = Response.data.data
              console.log(res.asset, "data trả về sau khi gán");
              console.log(Response.data.data, "data response")
              // debugger; // eslint-disable-line no-debugger
            })
            .catch((error) => {
              this.errorMessage = error.message;
              console.error("GET Asset by id Failed: ", error.message);
            });
        }
        else {
          this.resetInput()
        }
      },
      hide() {
        this.isActive = false;
        document.getElementsByClassName("body-right")[0].style.zIndex = "0";
      },
      getDepartmentName() {
        var res = this
        console.log(this.listAssetType, "list deparment");
        this.listDepartment.forEach(element => {
          console.log(element.departmentId, "element.departmentId");
          console.log(res.asset.departmentId, "res.asset.departmentId");
          if (element.departmentId == res.asset.departmentId) {
            console.log("vaof ddideu kien roi");
            res.asset.departmentName = element.departmentName
          }
        });
      },
      getAssetTypeName() {
        var res = this
        this.listAssetType.forEach(element => {

          if (element.assetTypeId == res.asset.assetTypeId) {
            res.asset.assetTypeName = element.assetTypeName
          }
        });
      },

      // todo định dạng tiền
      formatMoney(money) {
        money = parseInt(money).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.")
        // debugger; // eslint-disable-line no-debugger
        return money
      },


      /// todo chỉ cho phép nhập số
      handleKeyupOnlyNumber(event) {
        var accept = this.isNumber(event);
        if (accept == false) {
          event.preventDefault();
        }
      },

      /// todo kiểm tra có phải số hay không
      isNumber: function (evt) {
        evt = evt ? evt : window.event;
        var charCode = evt.which ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
          return false;
        }
        return true;
      },
     async validateAssetCode() {
        var warning = document.getElementById("assetInput1_warning")
        if (this.asset.assetCode == null || this.asset.assetCode == "") {
          warning.style.display = "block"
        }
        else if(this.formMode == 'insert')
        {
          
        }
        else {
          warning.style.display = "none"
        }
      },
      validateAssetName() {
        var warning = document.getElementById("assetInput2_warning")
        if (this.asset.assetName == null || this.asset.assetName == "") {
          warning.style.display = "block"
        }
        else {
          warning.style.display = "none"
        }
      },
      validateform()
      {
        this.validateAssetCode()
        this.validateAssetName()
      }


    },
    watch: {
      "asset.departmentId": function () {
        this.getDepartmentName()
      },
      "asset.assetTypeId": function () {
        this.getAssetTypeName()
      },
    },
    filters: {

    }

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
    display: none
  }
</style>