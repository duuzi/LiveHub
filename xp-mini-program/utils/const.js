const BusinessType = [
  { id: 0, name: '出差' },
  { id: 1, name: '返程' },
  { id: 2, name: '中转' }
]
const AmOrPm = [
  { id: 0, name: '上午' },
  { id: 1, name: '下午' }
]
const DownliadDateType = [
  { id: 0, name: '昨日' },
  { id: 1, name: '今日' },
  { id: 2, name: '上周' },
  { id: 3, name: '本周' },
  { id: 4, name: '上月' },
  { id: 5, name: '本月' }
]
//const host = 'http://10.30.47.53:8007'
const host = 'http://localhost:53026'
module.exports={
  BusinessType,
  AmOrPm,
  host
}