<template>
  <div>
    <div v-if="isActive" class="div-container modal">
      <div class="modal-background"></div>
      <div class="modal-content">
        <div class="modal-header">Xóa bản ghi</div>
        <div class="modal-content" style="width: 318px">
          Bạn chắc chắn muốn xóa bản ghi này chứ?
        </div>
        <div class="modal-footer">
          <div class="btn" @click="$parent.toggleDeleteModal">Hủy</div>
          <div class="btn btn-confirm-delete" @click="this.confirmDelete">
            Xóa
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
   }
})
</script>


<style lang='scss' scoped>
.btn.btn-confirm-delete {
  background-color: #ff4646;
  color: white;
}
.btn.btn-confirm-delete:hover {
  background-color: #ff7777;
}

.modal {
  .modal-content {
    position: relative;
    top: calc(50% - 100px);
    left: calc(50% - 175px);
    width: 350px;
    height: 200px;
    background-color: white;

    .modal-header {
      height: 60px;
      padding: 0 16px;
      line-height: 60px;
      font-size: 18px;
      font-weight: 500;
    }

    .modal-content {
      height: calc(100% - 110px);
      padding: 16px;
    }
    .modal-footer {
      height: 50px;
      display: flex;
      align-items: center;
      justify-content: flex-end;
      background-color: #f5f5f5;
      padding: 0 10px;
      //TODO set auto focus vao nut xoa
      .btn {
        margin: 0 6px;
        height: 34px;
      }
    }
  }
}
</style>