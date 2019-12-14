<template>
<view>

  <view @tap="addToDo" class="add-wrap" hover-class="plus-hover">
    <image src="../../../static/images/alarmAdd.png"></image>
  </view>
  
  <view class="cu-list menu sm-border card-menu margin-top margin-bottom shadow-warp">
    <view class="cu-tag bg-blue text-bold">To Do</view>
    <view :class="'cu-item ' + (modalName==move-box-+item.id?'move-cur':'')" v-for="(item, index) in toDoList" :key="index" @touchmove="ListTouchMove" @touchend="ListTouchEnd" @touchstart="ListTouchStart" :data-target="'move-box-' + item.id" v-if="item.toDoType==0">
      <view class="content">
        <text :class="'text ' + (item.checked?'del-line':'')">{{item.name}}</text>
      </view>
      <view class="action">
        <checkbox-group @change="CheckBoxTap" :data-item="item">
          <checkbox class="round" :checked="item.checked"></checkbox>
        </checkbox-group>
      </view>
      <view class="move">
        <view class="bg-red" @tap="delToDo" :data-item="item">删除</view>
      </view>
    </view>
  </view>
  <view class="cu-list menu sm-border card-menu margin-top margin-bottom shadow-warp">
    <view class="cu-tag bg-orange text-bold">Things To Buy</view>
    <view :class="'cu-item ' + (modalName==move-box-+item.id?'move-cur':'')" v-for="(item, index) in toDoList" :key="index" @touchmove="ListTouchMove" @touchend="ListTouchEnd" @touchstart="ListTouchStart" :data-target="'move-box-' + item.id" v-if="item.toDoType==1">
      <view class="content">
        <text :class="'text ' + (item.checked?'del-line':'')">{{item.name}}</text>
      </view>
      <view class="action">
        <checkbox-group @change="CheckBoxTap" :data-item="item">
          <checkbox class="round" :checked="item.checked"></checkbox>
        </checkbox-group>
      </view>
      <view class="move">
        <view class="bg-red" @tap="delToDo" :data-item="item">删除</view>
      </view>
    </view>
  </view>
  <view class="cu-list menu sm-border card-menu margin-top margin-bottom shadow-warp">
    <view class="cu-tag bg-green text-bold">Training</view>
    <view :class="'cu-item ' + (modalName==move-box-+item.id?'move-cur':'')" v-for="(item, index) in toDoList" :key="index" @touchmove="ListTouchMove" @touchend="ListTouchEnd" @touchstart="ListTouchStart" :data-target="'move-box-' + item.id" v-if="item.toDoType==2">
      <view class="content">
        <text :class="'text ' + (item.checked?'del-line':'')">{{item.name}}</text>
      </view>
      <view class="action">
        <checkbox-group @change="CheckBoxTap" :data-item="item">
          <checkbox class="round" :checked="item.checked"></checkbox>
        </checkbox-group>
      </view>
      <view class="move">
        <view class="bg-red" @tap="delToDo" :data-item="item">删除</view>
      </view>
    </view>
  </view>

  <view :class="'cu-modal ' + (modalShow?'show':'')">
    <view class="cu-dialog">
      <view class="cu-bar bg-white justify-end">
        <view class="content">Add ToDo</view>
        <view class="action" @tap="hideModal">
          <text class="cuIcon-close text-red"></text>
        </view>
      </view>
      <form @submit="submitToDo">
      <view class="padding-xl">
          <input class="margin-bottom" name="toDoName" :value="toDoName" placeholder="请输入待办事项..."></input>
          <radio-group class="margin-top" name="toDoType" :value="toDoType" @change="checkboxChange">
            <label class="checkbox" v-for="(item, index) in toDoType" :key="index">
              <radio :value="item.id" :checked="item.checked" style="margin-left:20rpx"></radio> {{item.name}}
            </label>
          </radio-group>
      </view>
      <view class="cu-bar bg-white justify-end">
        <view class="action">
          <button class="cu-btn line-green text-green" @tap="hideModal">取消</button>
          <button formType="submit" class="cu-btn bg-green margin-left">确定</button>
        </view>
      </view>
      </form>
    </view>
  </view>
</view>
</template>

<script>
// pages/basics/todolist/todolist.js
import WxValidate from "../../../utils/WxValidate";
const util = require("../../../utils/util.js");
const api = require("../../../config/api.js");
const app = getApp().globalData;

export default {
  data() {
    return {
      modalName: '',
      modalShow: false,
      toDoList: [{
        id: 1,
        name: 'test1',
        status: 0,
        order: 1,
        toDoType: 0,
        checked: true
      }, {
        id: 2,
        name: 'test2',
        status: 0,
        order: 1,
        toDoType: 1,
        checked: false
      }, {
        id: 3,
        name: 'test3',
        status: 0,
        order: 1,
        toDoType: 2,
        checked: false
      }],
      // toDoList: {
      //   doList: [
      //     { id: 1, name: 'test1', status: 0, order: 1, checked: true },
      //     { id: 2, name: 'test2', status: 1, order: 1, checked: false }
      //   ], thingsList: [
      //     { id: 3, name: 'test3', status: 0, order: 1, checked: false },
      //     { id: 4, name: 'test4', status: 1, order: 1, checked: true }
      //   ], trainingList: [
      //     { id: 5, name: 'test5', status: 0, order: 1, checked: true },
      //     { id: 6, name: 'test6', status: 1, order: 1, checked: false }
      //   ]
      // },
      toDoType: [{
        id: 0,
        name: 'ToDo'
      }, {
        id: 1,
        name: 'ToBuy'
      }, {
        id: 2,
        name: 'Training'
      }],
      ListTouchStart: "",
      ListTouchDirection: "",
      toDoName: ""
    };
  },

  components: {},
  props: {},

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    this.initValidate();
    this.getToDoList();
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {},

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {
    //摇一摇删除已完成项目
    wx.onAccelerometerChange(function (res) {
      if (res.x > 1 && res.y > 1) {
        let list = this.toDoList;
        list = list.filter(_ => _.checked);

        if (list && list.length > 0) {
          let ids = '';

          for (let item of list) {
            ids += item.id + ',';
          }

          ids = ids.substring(0, ids.length - 1);
          let params = {
            ids: ids
          };
          util.request(api.DelToDo, params, 'POST').then(res => {
            this.getToDoList();
            wx.showToast({
              title: '清除完成!'
            });
          });
        }
      }
    });
  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {},

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {},

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {},

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {},

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {},
  methods: {
    //获取初始数据
    getToDoList() {
      util.request(api.GetToDoList).then(res => {
        this.setData({
          toDoList: res.data
        });
      });
    },

    submitToDo(e) {
      if (!wx.getStorageSync('token')) {
        util.showErrorToast("请先登录!");
      }

      const params = e.detail.value;

      if (!this.WxValidate.checkForm(params)) {
        const error = this.WxValidate.errorList[0];
        this.showModal(error);
        return false;
      }

      util.request(api.AddToDo, params, 'POST').then(res => {
        this.getToDoList();
        this.setData({
          modalShow: false
        });
        wx.showToast({
          title: '添加成功!'
        });
      });
    },

    delToDo(e) {
      let list = this.toDoList;
      let item = e.currentTarget.dataset.item;
      let params = {
        ids: item.id
      };
      list.splice(list.findIndex(_ => _.id === item.id), 1);
      this.setData({
        toDoList: list
      });
      util.request(api.DelToDo, params, 'POST').then(res => {});
    },

    //点击checkbox
    CheckBoxTap(e) {
      let list = this.toDoList,
	      values = e.detail.value;

      if (e.detail.value.length > 0) {
        //已完成
        item.checked = true;
        item.status = 1;
      } else {
        //待办
        item.checked = false;
        item.status = 0;
      }

      list.find(_ => _.id == item.id).checked = item.checked;
      let params = item;
      params.toDoName = item.name;
      util.request(api.UpdToDo, item, 'POST').then(res => {
        this.setData({
          toDoList: list
        }); //this.getToDoList()
      });
      console.log('点击checkbox');
    },

    // ListTouch触摸开始
    ListTouchStart(e) {
      this.setData({
        ListTouchStart: e.touches[0].pageX
      });
    },

    // ListTouch计算方向
    ListTouchMove(e) {
      this.setData({
        ListTouchDirection: e.touches[0].pageX - this.ListTouchStart > 0 ? 'right' : 'left'
      });
    },

    // ListTouch计算滚动
    ListTouchEnd(e) {
      if (this.ListTouchDirection == 'left') {
        this.setData({
          modalName: e.currentTarget.dataset.target
        });
      } else {
        this.setData({
          modalName: null
        });
      }

      this.setData({
        ListTouchDirection: null
      });
    },

    addToDo: function (e) {
      this.setData({
        modalShow: true,
        toDoName: ''
      });
      console.log('tap addtodo');
    },

    hideModal(e) {
      this.setData({
        modalShow: false
      });
    },

    showModal(error) {
      wx.showModal({
        content: error.msg,
        showCancel: false
      });
    },

    initValidate() {
      const rules = {
        toDoName: {
          required: true,
          maxlength: 32
        },
        toDoType: {
          required: true
        }
      };
      const messages = {
        toDoName: {
          required: '请输入待办事项',
          maxlength: '不能超过32个字'
        },
        toDoType: {
          required: '请选择类型'
        }
      };
      this.WxValidate = new WxValidate(rules, messages);
    },

    setData: function (obj, callback) {
      let that = this;
      let keys = [];
      let val, data;
      Object.keys(obj).forEach(function (key) {
        keys = key.split('.');
        val = obj[key];
        data = that.$data;
        keys.forEach(function (key2, index) {
          if (index + 1 == keys.length) {
            that.$set(data, key2, val);
          } else {
            if (!data[key2]) {
              that.$set(data, key2, {});
            }
          }

          data = data[key2];
        });
      });
      callback && callback();
    }
  }
};
</script>
<style>
@import "./todolist.css";
</style>