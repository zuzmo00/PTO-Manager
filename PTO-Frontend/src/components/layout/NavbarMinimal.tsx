import {
    IconAdjustmentsStar,
    IconAlarm, IconChartDots, IconEditOff,
    IconGitPullRequest,
    IconHome,
    IconLogout, IconPercentage25, IconUsers,
} from "@tabler/icons-react";
import classes from "./NavbarMinimalColored.module.css";
import { rem, Button, useMantineTheme, Image } from "@mantine/core";
import { useMediaQuery } from "@mantine/hooks";
import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import useAuth from "../../hooks/useAuth.tsx";
import { AuthContext } from "../../context/AuthContext.tsx";

interface NavbarLinkProps {
    icon: typeof IconHome;
    label: string;
    color: string;
    active?: boolean;
    onClick?(): void;
}

function NavbarLink({ icon: Icon, label, color, active, onClick }: NavbarLinkProps) {
    return (
        <div
            role="button"
            className={classes.link}
            color={color}
            onClick={onClick}
            data-active={active || undefined}
        >
            <Button
                variant="light"
                color={color}
                className={classes.iconButton}
                style={{ width: rem(40), height: rem(40), flexGrow: 0, flexShrink: 0, flexBasis: rem(40) }}
            >
                <Icon
                    className={classes.linkIcon}
                    style={{ width: rem(25), height: rem(25), flexGrow: 0, flexShrink: 0, flexBasis: rem(25) }}
                    stroke={1.8}
                />
            </Button>
            <span>{label}</span>
        </div>
    );
}

type NavbarMinimalProps = {
    toggle?: () => void;
};

export function NavbarMinimal({ toggle = () => {} }: NavbarMinimalProps) {
    const theme = useMantineTheme();
    const isMobile = useMediaQuery(`(max-width: ${theme.breakpoints.sm})`);
    const [active, setActive] = useState(0);
    const navigate = useNavigate();
    const { logout } = useAuth();
    const { role } = useContext(AuthContext);

    const menuItems = [
        {
            icon: IconHome,
            label: "Kezdőlap",
            url: "Landing",
            roles: ["User","Administrator"],
        },
        {
            icon: IconGitPullRequest,
            label: "Kérelem leadása",
            url: "Request",
            roles: ["User","Administrator"],
        },
        {
            icon: IconAlarm,
            label: "Kérelemek bírálata",
            url: "Deciding",
            roles: ["Administrator"],
        },
        {
            icon: IconEditOff,
            label: "Elfogadott bírálatok módosítása",
            url: "AcceptedRequestManage",
            roles: ["Administrator"],
        },
        {
            icon: IconChartDots,
            label: "Statisztikai adatok",
            url: "StatisticsPage",
            roles: ["Administrator"],
        },
        {
            icon: IconUsers,
            label: "Dolgozók kezelése",
            url: "ManageWorkers",
            roles: ["Administrator"],
        },
        {
            icon: IconAdjustmentsStar,
            label: "Különleges napok kezelése",
            url: "ManageSpecialDays",
            roles: ["Administrator"],
        },
        {
            icon: IconPercentage25,
            label: "Részlegek kezelése",
            url: "ManageDepartment",
            roles: ["Administrator"],
        },

    ];

    const onLogout = () => {
        logout();
    };

    useEffect(() => {
        const idx = menuItems.findIndex((c) => location.pathname.endsWith(c.url));
        if (idx >= 0) setActive(idx);
    }, []);

    const links = menuItems
        .filter((item) => item.roles.includes(role ?? ""))
        .map((link, index) => (
            <NavbarLink
                color="app-color"
                {...link}
                key={link.label}
                active={index === active}
                onClick={async () => {
                    setActive(index);
                    toggle();
                    navigate(link.url);
                }}
            />
        ));

    return (
        <nav className={classes.navbar}>
            <Image  src="/PTO_Manager_ver2.png" alt="logo" w={150} px={5} mb={20} m="auto" />
            <div className={classes.navbarMain}>{links}</div>
            <div className={classes.footer} style={{ width: !isMobile ? "216px" : "90%" }}>
                <NavbarLink icon={IconLogout} label={"Kijelentkezés"} onClick={onLogout} color="grape" />
            </div>
        </nav>
    );
}

export default NavbarMinimal;