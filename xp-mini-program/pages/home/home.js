const util = require('../../utils/util.js');
Page({
  options: {
    addGlobalClass: true,
  },
  data: {
    elements: [{
      title: '体重指数',
      name: 'bmi',
      color: 'cyan',
      icon: 'footprint'
    },
    {
      title: '待办事项',
      name: 'todolist',
      color: 'blue',
      icon: 'list'
    },
    {
      title: '学习',
      name: '2048',
      color: 'purple',
      icon: 'game'
    },
    {
      title: '签到 ',
      name: 'signIn',
      color: 'mauve',
      icon: 'write'
    },
    // {
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
    ],
  },
  onLoad:function(){
    
  },
  onShow:function(){
    if (!wx.getStorageSync('token')) {
      
      wx.switchTab({
        url: '../user/index/index',
        success: res => {
          console.log(res);
        },
        fail: err => {
          console.log(err)
        }
      })
      setTimeout(() => {
        util.showErrorToast("请先登录!")
      }, 0)
    }
  },
  onShareAppMessage: function () {

  },
})