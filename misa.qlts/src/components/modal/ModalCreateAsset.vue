<template>
  <div>
    <div v-if="isActive" class="modal">
      <div class="modal-background"></div>
      <div class="modal-content">
        <div class="header">
          <div class="title">Thêm mới tài sản</div>

          <div class="header-right">
            <div class="icon-help btn btn-help"></div>
            <div
              class="icon-cancel btn btn-close"
              @click="$parent.toggleAddModal()"
            ></div>
          </div>
        </div>

        <div class="content">
          <div class="input-field">
            <label for="">Mã tài sản (<span style="color: red">*</span>)</label>
            <input
              id="assetInput1"
              
              type="text"
              @keyup.enter="onClickBtnSubmit"
              maxlength="20"
              class="input-one-third"
              validateRequired="true"
              @keypress="handleKeypressNumberAndCharacter($event)"
              @blur="validateCodeField('assetInput1')"
              @keyup="onInputChangeRequired('assetInput1')"
            />

            <div id="assetInput1_warning" class="validate-warning">
              Không được để trống
            </div>
          </div>
          <div class="input-field">
            <label for=""
              >Tên tài sản (<span style="color: red">*</span>)</label
            >
            <input
              id="assetInput2"
              maxlength="255"
              validateRequired="true"
              class="input-two-third"
              type="text"
              name=""
              @keyup.enter="onClickBtnSubmit"
              @blur="onInputBlurRequired('assetInput2')"
              @keyup="onInputChangeRequired('assetInput2')"
            />
            <div id="assetInput2_warning" class="validate-warning">
              Không được để trống
            </div>
          </div>
          <div class="clear-float"></div>
          <div class="input-field">
            <label @click="this.setCursorDefault" for="">Mã phòng ban</label>
            <v-autocomplete
              id="assetInput3"
              class="custom-autocomplete input-one-third"
              :items="listDepartmentProp"
              item-value="departmentId"
              item-text="departmentCode"
            ></v-autocomplete>
          </div>
          <div class="input-field">
            <label for="">Tên phòng ban</label>
            <input
              class="input-black input-two-third"
              tabindex="-1"
              type="text"
              name=""
              @keyup.enter="onClickBtnSubmit"
            />
          </div>
          <div class="clear-float"></div>
          <div class="input-field">
            <label for="">Mã loại tài sản</label>
            <v-autocomplete
              id="assetInput4"
              class="custom-autocomplete input-one-third"
              :items="listAssetTypeProp"
              item-value="assetTypeId"
              item-text="assetTypeCode"
            ></v-autocomplete>
          </div>
          <div class="input-field">
            <label for="">Tên loại tài sản</label>
            <input
              class="input-black input-two-third"
              tabindex="-1"
              type="text"
              name=""
              @keyup.enter="onClickBtnSubmit"
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
              @keyup.enter="onClickBtnSubmit"
              @keypress="handleKeyupOnlyNumber($event)"
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
              @keyup.enter="onClickBtnSubmit"
              @keypress="handleKeyupOnlyNumber($event)"
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
              @keyup.enter="onClickBtnSubmit"
              @keyup="onKeyupMoneyField('assetInput7')"
              @keypress="handleKeyupOnlyNumber($event)"
              @focus="onFocusMoneyField($event)"
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
              @keyup.enter="onClickBtnSubmit"
              @keypress="handleKeyupOnlyNumber($event)"
              @focus="onFocusMoneyField($event)"
            />
          </div>
        </div>
        <div class="footer">
          <div
            class="btn btn-cancel"
            tabindex="0"
            @keyup.enter="$parent.toggleAddModal()"
            @click="$parent.toggleAddModal()"
          >
            Hủy
          </div>
          <div
            class="btn btn-save"
            tabindex="0"
            @click="onClickBtnSubmit"
            @keyup.enter="onClickBtnSubmit"
            v-if="isInserting"
          >
            Thêm
          </div>
          <div
            class="btn btn-save"
            tabindex="0"
            @keyup.enter="onClickBtnSubmit"
            @click="onClickBtnSubmit"
            v-else
          >
            Lưu
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>

export default ({
   data(){
       return{
           isActive:false
       }
   },
   methods:{
     show()
     {
       this.isActive = true
     },
     hide()
     {
       this.isActive = false
     }
   }
})
</script>


<style lang='scss' scoped>
.test-autocomplete {
  border: 1px solid black;
}

.modal .validate-warning {
  font-family: GoogleSans-Regular;
  font-size: 13px;
  color: red;
  display: none;
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
</style>
