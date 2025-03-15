import { BaseAPIUrl } from "@/constants/authConstant";
import { Paths } from "@/constants/routePaths";
import axios from "axios";


const axiosInstance = axios.create({
  baseURL: BaseAPIUrl,
  withCredentials: true,
});

//request interceptor
axiosInstance.interceptors.request.use(
  function (config) {
    return config;
  },
  function (error) {
    return Promise.reject(error);
  },
);

let isRefreshing = false;
let failedQueue = [];

const processQueue = (error, token = null) => {
  failedQueue.forEach((prom) => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });

  failedQueue = [];
};

//response interceptor
axiosInstance.interceptors.response.use(
  function (response) {
    return response;
  },
  function (error) {
    const originalRequest = error.config;
    if (error.response.status === 401 && !originalRequest._retry) {
      location.href = Paths.Route_Home;
    }
    return Promise.reject(error);
  },
);

export default axiosInstance;