import {Center, Divider, Image, Paper, Text} from "@mantine/core";
import type {JSX} from "react";

interface AuthContainerInterface {
    children: JSX.Element;
}

const AuthContainer = ({children}: AuthContainerInterface) => {
    return <div className="auth-container">
        <Center><Image src="xyzdfgdfg" alt="img" w={150} mt={30}/></Center>
        <Center>
            <Paper radius="md" p="xl" withBorder maw={600} m={10}>
                <Text size="lg" fw={500}>
                    Üdvözlünk a Szabadságkezelő felületen!
                </Text>
                <Divider my="lg"/>
                {children}
            </Paper>
        </Center>
    </div>
}

export default AuthContainer;