<template>
  <div class="div-container content" id="content">
    <div class="div-container">
      <div class="content-nav">
        <div class="features-pane">
          <div class="features-pane-left">
            <input
              id="assetSearchBox"
              class="input-search"
              type="text"
              placeholder="Tìm kiếm theo tên, mã tài sản. "
            />
            <div class="icon-search"></div>
          </div>

          <div class="features-pane-right">
            <div
              class="btn-add-asset btn features-pane-item"
              @click="showDialog('insert', 0)"
            >
              Thêm
            </div>
            <div class="btn icon-refresh features-pane-item"></div>
            <div
              id="preventLeftClick"
              class="btn icon-trash features-pane-item"
            ></div>
          </div>
        </div>

        <div class="clear-float"></div>
      </div>

      <div id="loadBar"></div>
      <div class="content-grid grid">
        <table class="table-asset" id="tableAsset">
          <colgroup>
            <col width="50" />
            <!-- <col width="120" /> -->
            <col width="120" />
            <col min-width="800" />
            <col min-width="200" />
            <col min-width="500" />
            <col width="150" />
            <col width="100" />
          </colgroup>
          <thead>
            <tr>
              <th style="text-align: left">STT</th>
              <th
                sortProp="code"
                sortOrder="asc"
                id="columnAssetCode"
                class="hover-pointer"
                style="text-align: left"
              >
                mã tài sản
              </th>
              <th
                sortProp="name"
                sortOrder="asc"
                id="columnAssetName"
                class="hover-pointer"
                style="text-align: left"
              >
                Tên tài sản
              </th>
              <th
                sortProp="type"
                style="text-align: left"
                sortOrder="asc"
                id="columnAssetType"
                class="hover-pointer"
              >
                Loại tài sản
              </th>
              <th
                sortProp="department"
                sortOrder="asc"
                id="columnDepartment"
                class="hover-pointer"
                style="text-align: left"
              >
                Phòng ban
              </th>
              <th
                sortProp="price"
                sortOrder="asc"
                id="columnPrice"
                class="hover-pointer"
                style="text-align: right"
              >
                nguyên giá
              </th>
              <th style="text-align: left">chức năng</th>
            </tr>
          </thead>

          <tbody>
            <tr v-for="(asset, index) in listAsset"
             :key="asset.assetId"
              @click="selectRow(asset.assetId)"
              v-bind:class="selectedRow(asset.assetId) ? 'selected-row' : ''"
             >
              <td class="no-border-left">{{ index + 1 }}</td>
              <td>{{ asset.assetCode }}</td>
              <td>{{ asset.assetName }}</td>
              <td>{{ asset.assetTypeName }}</td>
              <td>{{ asset.departmentName }}</td>
              <td style="text-align: right">
                {{ asset.originalPrice | formatMoney(asset.originalPrice) }}
              </td>
              <td class="no-border-right">
                <div class="features-box">
                  <div
                    :id="'tableRow' + index + '_edit'"
                    class="table-icon icon-edit-pen"
                    @click="showDialog('update',asset.assetId)"
                  ></div>
                  <div
                    id="preventLeftClick"
                    class="table-icon icon-trash-table"
                    @click="showDeleteDialog(asset.assetId)"
                  ></div>
                  <div class="table-icon icon-refresh-time"></div>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
        <div class="ctx-menu" id="ctxMenu">
          <div class="ctx-menu-item">Thêm</div>
          <div class="ctx-menu-item">Sửa</div>
          <div id="preventLeftClick" class="ctx-menu-item">Xóa</div>
        </div>
      </div>

      <div class="table-summary">
        <div class="summary">
          <div class="asset-number">Tổng số tài sản:</div>
          <div class="price-number">Tổng nguyên giá:</div>
        </div>
      </div>

      <div id="assetPopup"></div>
      <!-- <BaseLoader /> -->
    </div>

    <ModalCreateAsset
      ref="ModalCreateAsset_ref"
      :listDepartment="listDepartment"
      :listAssetType="listAssetType"
      :formMode="formMode"
      :assetIdUpdate="assetIdUpdate"
      @reload="reload"
    />
    <ModalDeleteAsset @reload="reload" :assetIdUpdate="assetIdUpdate" ref="ModalDeleteAsset_ref"/>
  </div>
</template>

<script>
import ModalCreateAsset from "../modal/ModalCreateAsset.vue";
import ModalDeleteAsset from "../modal/ModalDeleteAsset.vue";
import axios from "axios";

export default {
  components: {
    ModalDeleteAsset,
    ModalCreateAsset,
  },
  data() {
    return {
      listAsset: [],
      asset: {
        assetId: null,
        assetCode: null,
        assetName: null,
        departmentId: null,
        assetTypeId: null,
        timeUse: null,
        wearRate: null,
        originalPrice: null,
        wearValue: null,
      },
      listDepartment: [],
      listAssetType: [],
      formMode: '',
      assetIdUpdate:null,
      listSelectRow:[]
    };
  },
  methods: {
    /**
     * Gửi request GET tới API
     * Author: TVThinh (12/5/2021)
     */
    async getAsset() {
      var res = this;
      await axios
        .get("https://localhost:44382/api/v1/Assets")
        .then((response) => {
          console.log("dữ liệu asset trả về là");
          console.log("data ne");
          console.log("data.data ne");
          res.listAsset = response.data.data;
          // debugger; // eslint-disable-line no-debugger
        });
    },

    /// todo hiển thị dialog thêm
    showDialog(text, Id) {
      if (text == "insert") {
        this.formMode = "insert";
      } else {
        this.formMode = "update";
        this.assetIdUpdate = Id
      }
      setTimeout(() => {
        this.$refs.ModalCreateAsset_ref.show();
      }, 300);
      // debugger; // eslint-disable-line no-debugger
    },
    /// todo lấy ra các phòng ban
    async getDepartment() {
      var res = this;
      await axios
        .get("https://localhost:44382/api/v1/Departments")
        .then((response) => {
          res.listDepartment = response.data.data;
        })
        .catch((error) => {
          this.errorMessage = error.message;
          console.error("GET Department Failed: ", error.message);
        });
    },

    ///  todo lấy ra các loại tài sản
    async getAssetType() {
      var res = this;
      await axios
        .get("https://localhost:44382/api/v1/AssetTypes")
        .then((response) => {
          res.listAssetType = response.data.data;
        })
        .catch((error) => {
          this.errorMessage = error.message;
          console.error("GET AssetType Failed: ", error.message);
        });
    },

    // todo tải lại dữ liệu
    reload(value)
    {
      if( value == false)
      {
        this.getAsset()
      }
    },

    //todo hiển thị form xác nhận xóa 
    showDeleteDialog(id)
    {
      this.assetIdUpdate = id
      this.$refs.ModalDeleteAsset_ref.show();
      console.log(id);
    },

 //  select hàng, nếu hàng đã được select thì xóa khỏi mẩng listSelectRow, và ngược lại
    selectRow(id) {
      var index = this.listSelectRow.indexOf(id);
      if (index > -1) {
        this.listSelectRow.splice(index, 1);
        return true;
      } else {
        this.listSelectRow.push(id);
        return false;
      }
    },

     // kiểm tra hàng đã được select hay chưa
    selectedRow(id) {
      if (this.listSelectRow.indexOf(id) > -1) return true;
      else return false;
    },
  },
  filters: {
    formatMoney: function (money) {
      if (Number.isInteger(money) == false) {
        console.log("Unable to convert to money: " + money);
        return "N/A";
      }
      var num = money.toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.");
      return num;
    },
  },
  created() {
    this.getAsset();
  },
  mounted() {
    this.getDepartment();
    this.getAssetType();
  },
 
};
</script>


<style lang='scss' scoped>
#assetPopup {
  padding: 0 40px;
  height: 80px;
  background-color: #b1fdb3;
  color: black;
  font-size: 16px;
  border-left: 10px solid #e2f2ff;
  display: flex;
  padding-left: 26px;
  align-items: center;
  position: absolute;
  right: -500px;
  bottom: 80px;
  transition: right 1s;
}
#ctxMenu {
  background-color: white;
  position: fixed;
  z-index: 100;
  // display: none;
}

.ctx-menu {
  font-size: 14px;
  box-shadow: 2px 2px 4px grey;
  display: none;

  .ctx-menu-item {
    padding: 8px 34px 8px 18px;

    &:hover {
      background-color: #eaeaea;
      cursor: pointer;
    }
  }
}
//TODO DATETIME theo định dạng d/m/y, bỏ chọn thời gian
//TODO Sử dụng được phím tab trong nhập liệu
//TODO Thay đổi font chữ của datetimepicker
//TODO Thêm khách hàng thành công;
#loadBar {
  width: 0;
  height: 3px;
  background-color: #00abfe;
  transition: 0.5s;
  margin-top: 8px;
  display: none;
  position: absolute;
}

// .content-nav {
//     position: relative;
// }

// .content-nav  :after {
//     content: '';
//     display: block;
//     border-bottom: 3px solid #00abfe;
//     width: 0;
//     position: absolute;
//     left: 0;
//     -webkit-transition: 1s ease;
//     transition: 1s ease;
// }

// .content-nav :hover:after {
//     width: 100%;
// }

.content-nav {
  height: 70px;
  padding: 16px;
  box-sizing: border-box;

  .features-pane {
    display: flex;
    position: relative;
    align-items: center;
    margin: 10px 0;
    // width: 100%;
    padding-right: 10px;
    font-size: 14px;

    input {
      height: 34px;
      width: 320px;
      box-sizing: border-box;
    }
    .btn {
      height: 34px;
    }

    .btn-search {
      margin-left: 10px;
    }

    .features-pane-right {
      position: absolute;
      right: 0;

      // background-color:lightsalmon;
      display: flex;
      .features-pane-item {
        margin: 0px 5px;
      }
    }

    .features-pane-left {
      display: flex;
      align-items: center;
      .icon-search {
        position: relative;
        right: 45px;

        &:hover {
          cursor: pointer;
          //TODO sau này thêm action tìm kiếm vào đây
        }
      }
    }
  }
}

.content-grid {
  margin: 16px 16px 16px 16px;
  height: calc(100% - 149px);
  position: relative;
  overflow: auto;
  box-sizing: border-box;
}

.not-active {
  display: none;
}

.features-box {
  display: flex;
  align-items: center;
  visibility: hidden;
  .table-icon {
    width: 15px;
    height: 15px;
    margin: 0 4px;
    &:hover {
      cursor: pointer;
    }
  }
}

.table-summary {
  font-size: 14px;
  font-family: GoogleSans-Medium;
  display: flex;
  position: relative;
  margin: 18px 16px 0 16px;
  height: 45px;

  .price-number {
    position: absolute;
    //TODO sẽ phải sửa lại cái này cho chuẩn với cột nguyên giá
    right: 120px;
  }
}
.content {
  height: calc(100vh - 60px);
  width: calc(100% - 200px);
  margin-left: 210px;
  transition: all 0.25s;
  background-color: white;
}

.content-nav {
  padding-top: 26px;
  padding-bottom: 6px;
}
.content-nav .features-pane {
  margin: 0px;
}
.div-container {
  position: relative;
}
.table-summary {
  font-size: 14px;
  font-family: GoogleSans-Medium;
  display: flex;
  /* position: relative; */
  position: absolute;
  bottom: 0px;
  /* margin: 18px 16px 0 16px; */
  height: 63px;
}
.content {
  height: calc(100vh - 68px);
  width: calc(100% - 200px);
  margin-left: 210px;
  transition: all 0.25s;
}

.content-grid {
  margin: 16px 16px 16px 16px;
  height: calc(100% - 165px);
  /* height: 100%; */
  position: relative;
  overflow: auto;
  box-sizing: border-box;
}
.table-summary {
  font-size: 14px;
  font-family: GoogleSans-Medium;
  display: flex;
  /* position: relative; */
  width: 100%;
  position: absolute;
  bottom: 0px;
  /* align-items: center; */
  /* margin: 18px 16px 0 16px; */
  height: 63px;
  padding: 18px 16px;
  box-sizing: border-box;
}
.table-summary .summary {
  height: 100%;
  width: 100%;
}

.content .div-container {
  height: 100%;
}

.content {
  position: relative;
  margin-left: 0;
  box-sizing: border-box;
  width: 100%;
  height: calc(100% - 60px);
}
.content-nav .features-pane input {
  font-style: italic;
}
table tr th {
  white-space: nowrap;
}

.selected-row {
  background: red !important;
}
</style>