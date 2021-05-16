<template>
  <div class="div-container content" id="content">
    <div class="div-container">
      <div class="content-nav">
        <v-alert
          id="success-dialog"
          v-show="isSuccess"
          color="green"
          elevation="13"
          type="success"
          >Thao tác thành công!</v-alert
        >

        <div class="features-pane">
          <div class="features-pane-left">
            <input
              id="assetSearchBox"
              class="input-search"
              type="text"
              placeholder="Tìm kiếm theo tên, mã tài sản. "
              v-model="inputSearch"
              @change="getAsset()"
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

            <div
              @click="getAsset()"
              class="btn icon-refresh features-pane-item"
            ></div>
            <div
              id="preventLeftClick"
              class="btn icon-trash features-pane-item"
              @click="showDeleteDialog()"
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
            <tr
              v-for="(asset, index) in listAsset"
              :key="asset.assetId"
              v-bind:class="selectedRow(asset.assetId) ? 'selected-row' : ''"
              @click="selectRow(asset.assetId, $event)"
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
                    @click="showDialog('update', asset.assetId)"
                    title="Sửa"
                  ></div>
                  <div
                    id="preventLeftClick"
                    class="table-icon icon-trash-table"
                    @click="showDeleteDialog()"
                    title="Xóa"
                  ></div>
                  <div
                    class="table-icon icon-refresh-time"
                    title="Chức năng chưa phát triển"
                  ></div>
                </div>
              </td>
            </tr>
          </tbody>
          <div v-if="getSuccess" class="loading-dialog">
            <div class="icon-loading"></div>
          </div>
          <div v-if="!isSuccess" class="loading-dialog-execu">
            <div class="icon-loading"></div>
          </div>
          <div v-if="getEmty" class="loading-emty">Không có dữ liệu</div>
        </table>
        <div class="ctx-menu" id="ctxMenu">
          <div class="ctx-menu-item">Thêm</div>
          <div class="ctx-menu-item">Sửa</div>
          <div id="preventLeftClick" class="ctx-menu-item">Xóa</div>
        </div>
      </div>

      <div class="table-summary">
        <div class="summary">
          <div class="asset-number">Tổng số tài sản: {{ amountAsset }}</div>
          <div class="price-number">
            Tổng nguyên giá: {{ totalPrice | formatMoney() }}
          </div>
        </div>
      </div>

      <div id="assetPopup"></div>
    </div>

    <ModalCreateAsset
      ref="ModalCreateAsset_ref"
      :listDepartment="listDepartment"
      :listAssetType="listAssetType"
      :formMode="formMode"
      :assetIdUpdate="assetIdUpdate"
      @reload="reload"
    />
    <ModalDeleteAsset
      @reload="reload"
      :listSelectRow="listSelectRow"
      ref="ModalDeleteAsset_ref"
    />
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
      formMode: "",
      assetIdUpdate: null,
      listSelectRow: [],
      listAssetId: [],
      inputSearch: "",
      isSuccess: false,
      isError: false,
      getSuccess: true,
      getEmty: false,
      totalPrice: 0,
      amountAsset: 0,
    };
  },
  methods: {
    /**
     * Gửi request GET tới API
     * Author: TVThinh (12/5/2021)
     */
    async getAsset() {
      var res = this;
      this.listSelectRow = [];
      this.getSuccess = true;
      this.getEmty = false;
      this.amountAsset = 0;
      this.totalPrice = 0;
      await axios
        .get(
          "https://localhost:44382/api/v1/Assets/Filter/?input=" +
            res.inputSearch
        )
        .then((response) => {
          res.listAsset = response.data.data;
          if (res.listAsset.length == 0) {
            res.getEmty = true;
          }
          res.getSuccess = false;
          res.listAssetId = [];
          res.listAsset.forEach((element) => { // duyệt qua tất cả các bản ghi
            res.listAssetId.push(element.assetId); // push tất cả id tài sản vào mảng
            res.amountAsset++; // đếm tổng số bản ghi
            if (element.originalPrice != null) {
              res.totalPrice += parseInt(element.originalPrice); // tính tổng nguyên giá
            }
          });
          // debugger; // eslint-disable-line no-debugger
        })
        .catch((error) => {
          console.log(error);
          setTimeout(() => {
            res.getSuccess = false; // tắt dialog loading
            res.getEmty = true;  // b
          }, 3000);
        });
    },

    /// todo hiển thị dialog thêm
    showDialog(text, Id) {
      if (text == "insert") {
        this.formMode = "insert";
      } else {
        this.formMode = "update";
        this.assetIdUpdate = Id;
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
    reload(value) {
      if (value == true) {
        this.getAsset();
        this.isSuccess = true;
        setTimeout(() => {
          this.isSuccess = false;
        }, 6000);
      }
    },

    //todo hiển thị form xác nhận xóa
    showDeleteDialog() {
      this.$refs.ModalDeleteAsset_ref.show();
    },

    //  select hàng, nếu hàng đã được select thì xóa khỏi mẩng listSelectRow, và ngược lại
    selectRow(id, event) {
      if (event.ctrlKey == false && event.shiftKey == false) {
        this.listSelectRow = [];
        this.listSelectRow.push(id);
      } else if (event.shiftKey) {
        var idFirst = this.listSelectRow[0];
        this.listSelectRow = [];
        this.listSelectRow.push(idFirst);

        // vị trí đầu tiên trong mảng listSelectRow
        var idStart = this.listSelectRow[0];
        var indexStart = this.listAssetId.indexOf(idStart);

        var indexEnd = this.listAssetId.indexOf(id);
        if (indexStart > indexEnd) {
          indexStart--;
          var tem = indexStart;
          indexStart = indexEnd;
          indexEnd = tem;
        } else {
          indexStart++;
        }

        for (var i = indexStart; i <= indexEnd; i++) {
          this.listSelectRow.push(this.listAssetId[i]);
        }
      } else if (event.ctrlKey) {
        var index = this.listSelectRow.indexOf(id);
        if (index > -1) {
          this.listSelectRow.splice(index, 1);
        } else {
          this.listSelectRow.push(id);
        }
      }

      console.log(event.keyCode, "các sự kiện keycode là");
    },

    // kiểm tra hàng đã được select hay chưa
    selectedRow(id) {
      if (this.listSelectRow.indexOf(id) > -1) return true;
      else return false;
    },

    // todo xử lý sự kiện mũi tên lên xuống để select row
    processkey() {
      var res = this;
      window.addEventListener("keydown", function (e) {
        var len1 = res.listSelectRow.length; // số phần tử của mảng listSelectRow
        var len2 = res.listAssetId.length; //số phần tử của mảng listAssetId
        switch (e.keyCode) {
          case 38:
            {
              //up arrow
              if (
                len1 == 0 ||
                res.listAssetId.indexOf(res.listSelectRow[0]) == 0
              ) {
                res.listSelectRow = [];
                res.listSelectRow.push(res.listAssetId[len2 - 1]);
              } else if (len1 > 0) {
                //res.listSelectRow = [];
                var indexIdFirst = res.listAssetId.indexOf(
                  res.listSelectRow[0]
                );
                res.listSelectRow = [];
                res.listSelectRow.push(res.listAssetId[indexIdFirst - 1]);
              }
            }
            break;
          case 40:
            {
              // //down arrow
              if (
                len1 == 0 ||
                res.listAssetId.indexOf(res.listSelectRow[len1 - 1]) == len2 - 1
              ) {
                res.listSelectRow.push(res.listAssetId[0]);
              } else if (len1 > 0) {
                //res.listSelectRow = [];
                var indexIdLast = res.listAssetId.indexOf(
                  res.listSelectRow[len1 - 1]
                );
                res.listSelectRow = [];
                res.listSelectRow.push(res.listAssetId[indexIdLast + 1]);
              }
            }
            break;
          default: {
            return true;
          }
        }
      });
    },
  },
  filters: {
    formatMoney: function (money) {
      if (money != null)
        var num = money.toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.");
      else return "0";
      return num;
    },
  },
  created() {
    this.getAsset();
    this.processkey();
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
  user-select: none;
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

.v-sheet.v-alert {
  position: absolute;
  z-index: 2;
  right: 0px;
  top: 119px;
  animation-name: alert;
  width: 0px;
  animation-duration: 4s;
  white-space: nowrap;
  padding: 16px 0px;
}

@keyframes alert {
  0% {
    width: 0px;
  }
  50% {
    width: 200px;
    padding: 16px;
  }
  100% {
    width: 0px;
    animation-duration: 2s;
    padding: 16px 0px;
  }
}

.loading-dialog {
  position: absolute;
  height: 100%;
  width: 100%;
  opacity: 0.5;
  top: 34px;
  background: black;
  display: flex;
  align-items: center;
}

.loading-dialog .icon-loading {
  background-image: url("../../assets/UI/Icon/loading.svg");
  height: 80px;
  width: 80px;
  background-size: contain;
  margin: auto;
}
.loading-emty {
  font-weight: bold;
  width: inherit;
  display: flex;
  height: 138px;
  justify-content: center;
  align-items: center;
  font-size: 24px;
  position: absolute;
  top: 0;
}
table tbody tr td {
  font-family: "GoogleSans";
}
.summary {
  display: flex;
}
</style>