import { RouterProvider } from "react-router-dom";
import router from "./route";
import { useState } from "react";
import { AuthContext } from "./contexts/AuthContext";
// import { AuthContext } from "./contexts/authContext";

const App = () => {
  const [isAuthorized, setIsAuthorized] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [userInfo, setUserInfo] = useState(null);

  return (
    <>
      <AuthContext.Provider
        value={{ isAuthorized, userInfo, setUserInfo, setIsAuthorized }}
      >
        <RouterProvider router={router} />
      </AuthContext.Provider>
    </>
  );
};

export default App;
