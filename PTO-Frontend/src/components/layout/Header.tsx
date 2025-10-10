import {Burger, Flex, Image} from "@mantine/core";

type HeaderProps = {
    opened: boolean;
    toggle: () => void;
};

//Itt azért kell deklarálni ezt a typeot, mert így lehet biztosítani a várt érték típusát
const header = ({opened,toggle}: HeaderProps)=> {
    return (
        <Flex
            justify="space-between"
            style={{display: "flex", alignItems: "center", height: "100%", paddingLeft: '20px', paddingRight: '20px'}}
        >
            <Image src="" alt="Binder logo" w={80} px={5}/>
            <Burger
                opened={opened}
                onClick={toggle}
                hiddenFrom="sm"
                size="sm"
                mx={15}
            />
        </Flex>
    )
}
export default header;