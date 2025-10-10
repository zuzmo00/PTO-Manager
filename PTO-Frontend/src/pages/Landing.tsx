
import {Box, Center, Container, Divider, LoadingOverlay, Paper, Title} from "@mantine/core";
import {useEffect, useState} from "react";



function Landing(){

    const [isLoading, setIsLoading] = useState(false);

    const fetchData = async () => {
        setIsLoading(false);
    }

    useEffect(() => {
        fetchData();
    },[])

    return (
        <Container>
            <Paper p="xl" radius="md" withBorder pos = "relative">
                <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />
                <Center>
                    <Box style = {{textAlign: "center"}}>
                        <Title size="lg" mb="md">
                            Üdvözöljük a Szabadságkezelő Oldalon!
                        </Title>
                        <Divider my="lg"/>

                    </Box>
                </Center>

            </Paper>
        </Container>
    );
}

export default Landing;
