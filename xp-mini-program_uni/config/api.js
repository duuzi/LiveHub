//const ApiRootUrl = 'http://localhost:53026/'
const isSeven = true;
//const ApiRootUrl = 'https://api.codexp.club/'; 
const ApiRootUrl = 'http://localhost:53026/'
//const ApiRootUrl = 'http://10.30.47.200:8007/'

module.exports = {
  IsSeven: isSeven,
  AuthLoginByWeixin: ApiRootUrl + 'WxOpen/LoginByWeixin',
  //微信登录
  SignIn: ApiRootUrl + 'api/Business/SignIn',
  GetBusinessList: ApiRootUrl + 'api/Business/GetBusinessList',
  GetUser: ApiRootUrl + 'api/Account/GetUser',
  UpdateUser: ApiRootUrl + 'api/Account/UpdateUser',
  DownloadExcel: ApiRootUrl + 'api/Business/DownloadExcel',
  //livehub
  GetToDoList: ApiRootUrl + 'api/Business/GetToDoList',
  AddToDo: ApiRootUrl + 'api/Business/AddToDo',
  UpdToDo: ApiRootUrl + 'api/Business/UpdToDo',
  DelToDo: ApiRootUrl + 'api/Business/DelToDo',
  UploadScore: ApiRootUrl + 'api/Business/UploadScore',
  GetGameRank: ApiRootUrl + 'api/Business/GetGameRank',
  AddSignIn: ApiRootUrl + 'api/Business/AddSignIn',
  CheckSignIn: ApiRootUrl + 'api/Business/CheckSignIn',
  GetSignInList: ApiRootUrl + 'api/Business/GetSignInList',
  AuthLoginByApp:ApiRootUrl + 'api/Account/LoginByApp',
  AuthRegByApp:ApiRootUrl + 'api/Account/Regist',
};