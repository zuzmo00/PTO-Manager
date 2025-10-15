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
import type RequestAddAsUserDto from "../Interfaces/RequestAddAsUserDto.ts";
import {useNavigate} from "react-router-dom";

function Request () {

    dayjs.extend(isSameOrBefore);

    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string|null>(null);
    const [value, setValue] = useState<[string | null, string | null]>([null, null]);
    const today = dayjs().add(1,"day").startOf("day").toDate();
    const [reservedDays, setReservedDays] = useState<ReservedDays[]> ([]);
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

    const navigate = useNavigate();
    const handleConfirm = async () =>{
        if(!value[0] || !value[1]){
            notifications.show({
                title: "Kérelem leadás",
                message: "Hiba a kérelem leadása közben!",
                color: "red"
            })
            close();
        }else{
            setIsLoading(true)
            try{
                const inputrequestdto:RequestAddAsUserDto = {begin_Date: value[0], end_Date: value[1]}
                await api.Request.postCreateRequest(inputrequestdto);
                close()
                navigate('/')
            }
            catch (error){
                console.log(error);
                notifications.show({
                    title: "Hiba",
                    message: "Hiba a kérelem rögzítése közben!",
                    color: "red"
                })
            }finally {
                setIsLoading(false);
            }
        }

    }

    return(
      <Container>
          <LoadingOverlay visible={isLoading} overlayProps={{ blur: 2 }} />
          <Paper p="xl" radius="md" withBorder pos = "relative">
              <Center>
                  <Title style={{fontSize:20}}>Kérem válassza ki az időszakot amelyre szabadságot szeretne kérni!</Title>
              </Center>
              <Center mt={10} ta={"center"}>
                  <Box>
                      <Text>Évi összes szabadságnapok száma: {remainingDays?.allHoliday}</Text>
                      <Text>Fennmaradó szabadságok száma: {remainingDays?.remainingDays}</Text>
                      <Text>Időarányosan kivehető szabadságok száma:  {remainingDays?.timeProportional}</Text>
                  </Box>
              </Center>
              <Center>
                  {error && <Text c="red">{error}</Text>}
              </Center>
              <Center mt={20}>
                  <Box style = {{textAlign: "center"}}>
                      <DatePickerInput
                          type="range"
                          allowSingleDateInRange
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
                              <Button style={{backgroundColor:"red"}} disabled={isLoading} onClick={close}>Mégse</Button>
                              <Button onClick={handleConfirm} disabled={isLoading}>Leadás</Button>
                          </Group>
                      </Box>
                  </Box>
              </Center>
          </Modal>


      </Container>
    );
}

export default Request;