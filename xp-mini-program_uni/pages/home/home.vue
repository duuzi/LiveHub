<template>
<scroll-view scroll-y class="scrollPage">
  <image src="../../static/images/homebg.jpg" mode="widthFix" class="png" style="width:100%;height:486rpx"></image>
  <view class="nav-list margin-top">
    <navigator open-type="navigate" hover-class="none" :url="'/pages/basics/' + item.name + '/' + item.name" :class="'nav-li bg-' + item.color" v-for="(item, index) in elements" :key="index">
      <view class="nav-title">{{item.title}}</view>
      <view class="nav-name">{{item.name}}</view>
      <text :class="'cuIcon-' + item.icon"></text>
    </navigator>
  </view>
  <view class="cu-tabbar-height"></view>
</scroll-view>
</template>

<script>
const util = require("../../utils/util.js");

export default {
  data() {
    return {
      options: {
        addGlobalClass: true
      },
      elements: [{
        title: '体重指数',
        name: 'bmi',
        color: 'cyan',
        icon: 'footprint'
      }, {
        title: '待办事项',
        name: 'todolist',
        color: 'blue',
        icon: 'list'
      }, {
        title: '学习',
        name: '2048',
        color: 'purple',
        icon: 'game'
      }, {
        title: '签到 ',
        name: 'signIn',
        color: 'mauve',
        icon: 'write'
      } // {
      //   title: '按钮',
      //   name: 'button',
      //   color: 'pink',
      //   icon: 'btn'
      // },
      // {
      //   title: '标签',
      //   name: 'tag',
      //   color: 'brown',
      //   icon: 'tagfill'
      // },
      // {
      //   title: '头像',
      //   name: 'avatar',
      //   color: 'red',
      //   icon: 'myfill'
      // },
      // {
      //   title: '进度条',
      //   name: 'progress',
      //   color: 'orange',
      //   icon: 'icloading'
      // },
      // {
      //   title: '边框阴影',
      //   name: 'shadow',
      //   color: 'olive',
      //   icon: 'copy'
      // },
      // {
      //   title: '加载',
      //   name: 'loading',
      //   color: 'green',
      //   icon: 'loading2'
      // },
      ]
    };
  },

  components: {},
  props: {},
  onLoad: function () {},
  onShow: function () {
    if (!wx.getStorageSync('token')) {
      wx.switchTab({
        url: '../user/index/index',
        success: res => {
          console.log(res);
        },
        fail: err => {
          console.log(err);
        }
      });
      setTimeout(() => {
        util.showErrorToast("请先登录!");
      }, 0);
    }
  },
  onShareAppMessage: function () {},
  methods: {
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