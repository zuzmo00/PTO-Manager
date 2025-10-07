import { AppShell, Burger } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { NavbarMinimal } from "./NavbarMinimal";
import { Outlet } from "react-router-dom";

const BasicLayout = () => {
    const [opened, { toggle, close }] = useDisclosure(false);

    return (
        <AppShell
            navbar={{
                width: 250,
                breakpoint: "sm",
                collapsed: { mobile: !opened, desktop: false },
            }}
            padding="md"
            style={{ background: "#f9f9f9" }}
        >
            <AppShell.Navbar style={{ border: "none" }}>
                {/* linkre kattintva csukjuk be mobilon */}
                <NavbarMinimal toggle={close} />
            </AppShell.Navbar>

            <AppShell.Main style={{ background: 'url("/bg.png") no-repeat center center fixed' }}>
                {/* Lebegő Burger: csak mobilon és csak akkor látszik, ha a navbar rejtve van */}
                {!opened && (
                    <Burger
                        opened={opened}
                        onClick={toggle}
                        hiddenFrom="sm"
                        size="md"
                        aria-label="Menü megnyitása"
                        style={{ position: "fixed", top: 12, left: 12, zIndex: 1000 }}
                    />
                )}

                <Outlet />
            </AppShell.Main>
        </AppShell>
    );
};

export default BasicLayout;