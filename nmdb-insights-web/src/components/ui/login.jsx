"use client";
import React from "react";
import { useForm } from "react-hook-form";
import { Link, useNavigate } from "react-router-dom";
import { Paths } from "@/constants/routePaths";
import axiosInstance from "@/helpers/axiosSetup";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { useAuthContext } from "@/contexts/authContext";

const ErrorMessage = ({ message }) => (
  <p className="text-red-500 text-sm mt-1">{message}</p>
);

export default function LoginForm({ className, ...props }) {
  const { setIsAuthorized, setUserInfo } = useAuthContext();
  const navigate = useNavigate();
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm();

  const onSubmit = (data) => {
    axiosInstance
      .post("/auth/login", data) // Ensure the endpoint is correct
      .then((response) => {
        if (response?.data?.isSuccess) {
          setIsAuthorized(true);
          setUserInfo(response?.data?.user); //  set logged in user data which can be used across the app
          //reset(); // Reset the form fields
          navigate(Paths.Route_Dashboard);
        } else {
          console.error("Login unsuccessful");
          //reset(); // Reset the form fields even if login is unsuccessful
        }
      })
      .catch((error) => {
        console.error("Login error:", error);
        //reset(); // Reset the form fields in case of an error
      });
  };

  return (
    <div
      className={cn(
        "flex flex-col gap-6 w-full max-w-md mx-auto p-4",
        className
      )}
      {...props}
    >
      <Card className="overflow-hidden w-full">
        <CardContent className="p-6">
          <form onSubmit={handleSubmit(onSubmit)}>
            <div className="flex flex-col gap-6">
              <div className="flex flex-col items-center text-center">
                <img
                  src="/emblem_nepal.png"
                  alt="Emblem of Nepal"
                  className="w-24 h-24"
                />
                <p className="mt-3 text-sm text-balance text-muted-foreground">
                  Login to your NMDB account
                </p>
              </div>
              <div className="grid gap-2">
                <Label htmlFor="username">Email</Label>
                <Input
                  id="text"
                  type="text"
                  placeholder="email or username"
                  {...register("username", {
                    required: "Email is required",
                    // pattern: {
                    //   value: /\S+@\S+\.\S+/,
                    //   message: "Invalid email address",
                    // },
                  })}
                />
                {errors.email && (
                  <ErrorMessage message={errors.email.message} />
                )}
              </div>

              <div className="grid gap-2">
                <div className="flex items-center justify-between">
                  <Label htmlFor="password">Password</Label>
                  <Link
                    to="#"
                    className="text-xs underline-offset-2 hover:underline"
                  >
                    Forgot your password?
                  </Link>
                </div>
                <Input
                  id="password"
                  type="password"
                  {...register("password", {
                    required: "Password is required",
                    minLength: {
                      value: 8,
                      message: "Password must be at least 8 characters",
                    },
                  })}
                />
                {errors.password && (
                  <ErrorMessage message={errors.password.message} />
                )}
              </div>
              <Button type="submit" className="w-full" disabled={isSubmitting}>
                {isSubmitting ? "Logging in..." : "Login"}
              </Button>
            </div>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}
