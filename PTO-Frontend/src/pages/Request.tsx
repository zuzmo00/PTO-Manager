import {Box, Button, Center, Container, Group, LoadingOverlay, Modal, Paper, Text, Title} from "@mantine/core";
import { DatePickerInput  } from "@mantine/dates";
import { useDisclosure } from "@mantine/hooks";
import dayjs from "dayjs";
import isSameOrBefore from 'dayjs/plugin/isSameOrBefore'
import {useEffect, useState} from "react";
import api from "../api/api.ts";
import type ReservedDays from "../Interfaces/ReservedDays.ts";
import type RemainingDays from "../Interfaces/RemainingDays.ts";
import {notifications} from "@mantine/notifications";

function Request () {

    dayjs.extend(isSameOrBefore);

    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string|null>(null);
    const [value, setValue] = useState<[string | null, string | null]>([null, null]);
    const today = dayjs().add(1,"day").startOf("day").toDate();
    const [reservedDays, setReservedDays] = useState<ReservedDays[]> (null);
    const [remainingDays, setremainingDays] = useState<RemainingDays | null> (null);
    const [weekendWorkday, setWeekendWorkday] = useState<boolean>(false);


    const [modalOpened, { open, close }] = useDisclosure(false);

    const isValidRange = value[0] !== null &&
        value[1] !== null && dayjs(value[0]).isSameOrBefore(value[1]);


    useEffect(() => {
        fetchData();
        setWeekendWorkday(true);
    }, []);

    const fetchData = async () => {
        setIsLoading(true);

        try{
            const ReservedDays_response = await api.Request.getReservedDays();
            setReservedDays(ReservedDays_response.data?.data ?? [])

            const RemainingDays_response = await api.Request.getRemainingDays();
            setremainingDays(RemainingDays_response.data?.data ?? null)
        }
        catch(error){
            console.log(error);
            setError("Hiba történt a lekérdezés során")
        }
        finally {
            setIsLoading(false)
        }
    }

    const IsExcludedDate = (date:string) : boolean => {
        const temp_day = dayjs(date);
        const formated_day = temp_day.format("YYYY-MM-DD");

        let isWeekend:boolean = false

        const excludedDate = reservedDays.some(c=> c.reservedDay === formated_day);
        if(!weekendWorkday){
            isWeekend = temp_day.day() === 6 || temp_day.day() === 0;
        }

        return isWeekend || excludedDate;
    };

    const handleSubmit = () => {
        open();
    }

    const handleConfirm = () =>{
        if(!value[0] || !value[1]){
            notifications.show({
                title: "Kérelem leadás",
                message: "Hiba a kérelem leadása közben!",
                color: "red"
            })
        }
        setIsLoading(true)
        try{
            //await
        }
        catch (error){
            console.log(error);
            notifications.show({
                title: "Hiba",
                message: "Hiba a kérelem rögzítése közben!",
                color: "red"
            })
        }
    }

    return(
      <Container>
          <Paper p="xl" radius="md" withBorder pos = "relative">
              <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />
              <Center>
                  <Title style={{fontSize:20}}>Kérem válassza ki az időszakot amelyre szabadságot szeretne kérni!</Title>
              </Center>
              <Center mt={10}>
                  <Box>
                      <Text>Évi összes szabadságnapok száma: {remainingDays?.osszesSzab}</Text>
                      <Text>Eddig kivett szabadságok száma: {remainingDays?.eddigKivett}</Text>
                      <Text>Függőben lévő szabadságok száma:  {remainingDays?.fuggoben}</Text>
                  </Box>
              </Center>
              <Center>
                  {error && <Text c="red">{error}</Text>}
              </Center>
              <Center mt={20}>
                  <Box style = {{textAlign: "center"}}>
                      <DatePickerInput
                          type="range"
                          label="Időszak kiválasztása"
                          placeholder="Például: Október 20, 2025 - Október 24, 2025"
                          style={{minWidth: 200}}
                          value={value}
                          minDate={today}
                          onChange={setValue}
                          excludeDate={IsExcludedDate}
                          locale="hu"
                      />
                      <Box mt={20} style = {{textAlign: "center"}}>
                          <Button disabled={!isValidRange} onClick={handleSubmit} >
                              Kérelem leadása
                          </Button>
                      </Box>
                  </Box>
              </Center>
          </Paper>

          <Modal opened={modalOpened} onClose={close} centered title="Megerősítés" style={{textAlign: "center"}} >
              <Center>
                  <Box style={{textAlign:"center"}}>
                      <Text fw={"bold"}>Biztos, hogy le szeretné adni a kérelmet az alábbi időszakra?</Text>
                      <Text mt={10}>{value[0]}-tól   {value[1]}-ig</Text>
                      <Box>
                          <Group justify={"center"} mt={10}>
                              <Button style={{backgroundColor:"red"}}>Mégse</Button>
                              <Button onClick={handleConfirm}>Leadás</Button>
                          </Group>
                      </Box>
                  </Box>
              </Center>
          </Modal>


      </Container>
    );
}

export default Request;