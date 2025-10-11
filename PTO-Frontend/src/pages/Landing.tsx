import {Box, Center, Container, Divider, LoadingOverlay, Paper, Text, Title} from "@mantine/core";
import {useEffect, useState} from "react";
import { Calendar } from '@mantine/dates';
import dayjs from "dayjs";
import type ReservedDays from "../Interfaces/ReservedDays.ts";



function Landing(){

    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [foglaltNapok, setFoglaltNapok] = useState<ReservedDays[] | null>(null);
    setFoglaltNapok([
        {
            reservedDay: "2025-10-11",
            reservationType: 1
        },
        {
            reservedDay: "2025-10-14",
            reservationType: 2
        },
        {
            reservedDay: "2025-10-17",
            reservationType: 3
        },
        {
            reservedDay: "2025-10-20",
            reservationType: 1
        },
        {
            reservedDay: "2025-10-23",
            reservationType: 1
        },
    ])
    const [szabStat, setSzabStat] = useState<null | {
     osszeszab:string;
     fennmaradt:string;
     idoaranyos:string;
    }
    >(null);

    setSzabStat({
        osszeszab: "20",
        fennmaradt: "10",
        idoaranyos: "10"
        }
    )


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

                            let dayitem: ReservedDays | undefined = undefined
                            const isHetvegeMunkanap: boolean = true; //Majd ide kell egy api

                            const isFoglalt = foglaltNapok?.some(nap => nap.reservedDay === daystring);
                            if(isFoglalt){
                                dayitem = foglaltNapok?.find(cday => cday.reservedDay === daystring);
                            }


                            let backgorundColor: string | undefined;
                            let color : string | undefined;

                            if(isHetvegeMunkanap && (day.day() === 6 || day.day() === 0)){
                                backgorundColor = "gray"
                                color = "red"
                            }else if (!isFoglalt) {

                            }else if(dayitem?.reservationType === 5) { //SpecialWorkDay

                            }else if(dayitem?.reservationType === 6) { //SpecialHoliday

                            }

                        }}
                        />
                    </Box>
                </Center>

            </Paper>
        </Container>
    );
}

export default Landing;
