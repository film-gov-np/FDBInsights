import { Paths } from "@/constants/routePaths";
import axiosInstance from "@/helpers/axiosSetup";
import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";

const ErrorMessage = ({ message }) => (
  <p className="text-red-500 text-sm">{message}</p>
);

const LoginForm = () => {
  const navigate = useNavigate();
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    getValues,
  } = useForm({
    mode: "onSubmit",
  });

  const passwordRegex =
    /^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;

  const onSubmit = (data) => {
    const { email, password } = data;
    const postData = {
      email: email,
      password: password,
    };
    axiosInstance
      .post("/user/authenticate", postData)
      .then((resp) => {
        const response = resp.data;
        if (response?.success) {
          navigate(Paths.Route_Dashboard);
        } else {

        }
      })
      .catch((error) => {
        //to show error message
      });
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 px-4">
      <div className="bg-white p-8 rounded shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold mb-6 text-center">Login</h2>
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="mb-4">
            <label
              htmlFor="email"
              className="block text-gray-700 font-bold mb-2"
            >
              Email
            </label>
            <input
              type="email"
              id="email"
              {...register("email", {
                required: "Email is required",
                pattern: {
                  value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                  message: "Invalid email address",
                },
              })}
              className={`w-full px-3 py-2 border rounded focus:outline-none focus:border-blue-500 ${errors.email ? "border-red-500" : "border-gray-300"
                }`}
            />
            {errors.email && <ErrorMessage message={errors.email.message} />}
          </div>
          <div className="mb-4">
            <label
              htmlFor="password"
              className="block text-gray-700 font-bold mb-2"
            >
              Password
            </label>
            <input
              type="password"
              id="password"
              {...register("password", {
                required: "Password is required",
                pattern: {
                  value: passwordRegex,
                  message:
                    "Password must be at least 8 characters long and include at least one uppercase letter, one number, and one special character.",
                },
                validate: (value) => {
                  const email = getValues("email");
                  if (value === email) {
                    return "Email and password cannot be the same";
                  }
                  return true;
                },
              })}
              className={`w-full px-3 py-2 border rounded focus:outline-none focus:border-blue-500 ${errors.password ? "border-red-500" : "border-gray-300"
                }`}
            />
            {errors.password && (
              <div className="text-red-500 text-sm">
                {errors.password.type === "validate" && (
                  <ErrorMessage message={errors.password.message} />
                )}
                {errors.password.type === "pattern" && (
                  <ul className="list-disc list-inside">
                    <li>Password must be at least 8 characters long</li>
                    <li>Include at least one uppercase letter</li>
                    <li>Include at least one number</li>
                    <li>
                      Include at least one special character (e.g., @$!%*?&)
                    </li>
                  </ul>
                )}
              </div>
            )}
          </div>

          <button
            type="submit"
            disabled={isSubmitting}
            className="w-full bg-blue-500 text-white font-bold py-2 px-4 rounded hover:bg-blue-700 flex items-center justify-center"
          >
            {isSubmitting ? (
              <svg className="animate-spin h-5 w-5 mr-3" viewBox="0 0 24 24">
                <circle
                  className="opacity-25"
                  cx="12"
                  cy="12"
                  r="10"
                  stroke="currentColor"
                  strokeWidth="4"
                ></circle>
                <path
                  className="opacity-75"
                  fill="currentColor"
                  d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                ></path>
              </svg>
            ) : (
              "Login"
            )}
          </button>
        </form>
      </div>
    </div>
  );
};

export default LoginForm;
