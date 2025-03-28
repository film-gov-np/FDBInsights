import {
    Award,
    BriefcaseBusiness,
    ChevronDown,
    Clapperboard,
    Home,
    Projector,
    User,
    Users,
    Video,
} from "lucide-react";
import { NavLink } from "react-router-dom";
import { Badge } from "@/components/ui/badge";
import { Paths } from "@/constants/routePaths";
import { cn } from "@/lib/utils";
import useCheckActiveNav from "@/hooks/CheckActiveNav";
import { Collapsible, CollapsibleContent, CollapsibleTrigger } from "../ui/collapsible";

const sideMenuRoutes = [
    {
        title: "Dashboard",
        icon: Home,
        path: Paths.Route_Dashboard,
    },
    {
        title: "Analytics",
        icon: Video,
        path: Paths.Route_TheaterAnalytics
    },
];

const NavLinkCustom = ({
    path,
    title,
    label,
    isMobileSidebar,
    isSubLink,
    setOpen,
    ...rest
}) => {
    return (
        <div className={cn(isSubLink && "ps-4")}>
            <NavLink
                to={path}
                onClick={() => {
                    isMobileSidebar && setOpen(false);
                }}
                className={cn(
                    isMobileSidebar && "mx-[-0.65rem] gap-4",
                    "flex items-center gap-3 rounded-lg px-3 py-2 text-foreground transition-all hover:text-muted-foreground",
                )}
            >
                <rest.icon className={cn(isMobileSidebar ? "h-6 w-6" : "h-4 w-4")} />
                {title}
                {label && (
                    <Badge className="ml-auto flex h-6 w-6 shrink-0 items-center justify-center rounded-full">
                        {label}
                    </Badge>
                )}
            </NavLink>
        </div>
    );
};

const NavLinkDropdown = ({
    title,
    label,
    submenus,
    isMobileSidebar,
    setOpen,
    ...rest
}) => {
    const { checkActiveNav } = useCheckActiveNav();
    const isChildActive = !!submenus?.find((s) => checkActiveNav(s.path));
    return (
        <Collapsible defaultOpen={isChildActive}>
            <CollapsibleTrigger
                className={cn(
                    "group w-full",
                    (isMobileSidebar &&
                        "mx-[-0.65rem] flex items-center gap-4 rounded-xl px-3 py-2 text-muted-foreground hover:text-foreground") ||
                    " flex items-center gap-3 rounded-lg px-3 py-2 text-muted-foreground transition-all hover:text-foreground",
                )}
            >
                {<rest.icon className={cn(isMobileSidebar ? "h-6 w-6" : "h-4 w-4")} />}
                {title}
                {label && (
                    <Badge className="ml-auto flex h-6 w-6 shrink-0 items-center justify-center rounded-full">
                        {label}
                    </Badge>
                )}
                <span
                    className={cn(
                        "ml-auto transition-all group-data-[state='open']:-rotate-180",
                    )}
                >
                    <ChevronDown className=" h-4 w-4" />
                </span>
            </CollapsibleTrigger>
            <CollapsibleContent>
                <ul>
                    {submenus?.map((sublink) => (
                        <li key={sublink.title} className="my-1 ml-6">
                            <NavLinkCustom
                                {...sublink}
                                isMobileSidebar={isMobileSidebar}
                                setOpen={setOpen}
                            />
                        </li>
                    ))}
                </ul>
            </CollapsibleContent>
        </Collapsible>
    );
};

const SideMenu = ({ className, isMobileSidebar, setOpen }) => {
    return (
        <nav className={cn("sidebar-nav grid", className)}>
            {sideMenuRoutes.map(
                ({ submenus, ...route }, index) =>
                    (submenus && (
                        <NavLinkDropdown
                            key={route.title + index}
                            {...route}
                            submenus={submenus}
                            isMobileSidebar={isMobileSidebar}
                            setOpen={setOpen}
                        />
                    )) || (
                        <NavLinkCustom
                            key={route.title + index}
                            {...route}
                            isMobileSidebar={isMobileSidebar}
                            setOpen={setOpen}
                        />
                    ),
            )}
        </nav>
    );
};

export default SideMenu;