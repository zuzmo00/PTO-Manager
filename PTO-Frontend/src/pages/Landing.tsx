import {Box, Center, Container, Divider, LoadingOverlay, Paper, Text, Title} from "@mantine/core";
import {useEffect, useState} from "react";
import { Calendar } from '@mantine/dates';
import dayjs from "dayjs";



function Landing(){

    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [foglaltNapok, setFoglaltNapok] = useState<string[] | null>(null);
    const [szabStat, setSzabStat] = useState<null | {
     osszeszab:string;
     fennmaradt:string;
     idoaranyos:string;
    }
    >(null);

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
                        <Text>Kérjük válasszon az oldalsávon található menüpontok közül.</Text>
                        {error && <Text c="red">{error}</Text>}
                        {szabStat && (
                            <>
                                <Text>Összes szabadság: {szabStat.osszeszab} nap</Text>
                                <Text>Rendelkezésre álló: {szabStat.fennmaradt} nap</Text>
                                <Text>Időarányos: {szabStat.idoaranyos}</Text>
                            </>
                        )}
                        <Divider my="lg"/>
                    </Box>
                </Center>
                <Center>
                    <Box style = {{textAlign: "center"}}>
                        <Calendar
                        locale="hu"
                        getDayProps={(date) => {
                            const day = dayjs(date);
                            const daystring = day.format("YYYY-MM-DD");

                            const isFoglalt = foglaltNapok?.includes(date);

                            if (isFoglalt) {}

                        }}
                        />
                    </Box>
                </Center>

            </Paper>
        </Container>
    );
}

export default Landing;
